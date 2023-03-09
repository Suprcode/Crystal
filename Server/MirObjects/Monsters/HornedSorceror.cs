using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class HornedSorceror : MonsterObject
    {
        private long _TornadoTime;
        private long _ChargedStompTime;
        private bool _Immune;

        protected virtual byte AttackRange
        {
            get
            {
                return 5;
            }
        }

        protected internal HornedSorceror(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        public override bool IsAttackTarget(MonsterObject attacker)
        {
            return !_Immune && base.IsAttackTarget(attacker);
        }

        public override bool IsAttackTarget(HumanObject attacker)
        {
            return !_Immune && base.IsAttackTarget(attacker);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            AttackTime = Envir.Time + AttackSpeed;

            //Charged Stomp
            if (Envir.Time > _ChargedStompTime && HealthPercent < 90 && Envir.Random.Next(4) == 0)
            {
                byte stompLoops = (byte)Envir.Random.Next(5, 10);
                int stompDuration = stompLoops * 500;

                _ChargedStompTime = Envir.Time + 20000;

                ActionTime = Envir.Time + (stompDuration) + 500;
                AttackTime = Envir.Time + (stompDuration) + 500 + AttackSpeed;

                _Immune = true;

                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2, Level = stompLoops });

                int damage = GetAttackPower(Stats[Stat.MinSC], Stats[Stat.MaxSC]) * stompLoops;
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + stompDuration + 500, Target, damage, DefenceType.AC, true);
                ActionList.Add(action);
                return;
            }

            //Dust Tornado
            if (Envir.Time > _TornadoTime && HealthPercent < 90 && Envir.Random.Next(4) == 0)
            {
                _TornadoTime = Envir.Time + 15000;

                Tornado();
                return;
            }

            if (!ranged)
            {
                if (Envir.Random.Next(5) > 2) //Thrust hit
                {
                    ActionTime = Envir.Time + 300;

                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    LineAttack(damage, 2, 300);
                }
                else if (Envir.Random.Next(5) > 2) //Dust hit
                {
                    ActionTime = Envir.Time + 300;

                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                    if (damage == 0) return;

                    LineAttack(damage, 3, 300);
                }
                else
                {
                    ActionTime = Envir.Time + 500;

                    Thrust(Target);
                }
            }
            else
            {
                if (Envir.Random.Next(3) == 0)
                {
                    ActionTime = Envir.Time + 500;

                    Thrust(Target);
                }
                else
                {
                    MoveTo(Target.CurrentLocation);
                }
            }
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (InAttackRange() && CanAttack)
            {
                Attack();
                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.Front);
        }

        private void Tornado()
        {
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 3 });

            var location = CurrentLocation;

            for (int y = location.Y - 2; y <= location.Y + 2; y++)
            {
                if (y < 0) continue;
                if (y >= CurrentMap.Height) break;

                for (int x = location.X - 2; x <= location.X + 2; x++)
                {
                    if (x < 0) continue;
                    if (x >= CurrentMap.Width) break;

                    var cell = CurrentMap.GetCell(x, y);

                    if (!cell.Valid) continue;

                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MinMC]);

                    var start = 1000;
                    var time = Settings.Second * 15;

                    SpellObject ob = new SpellObject
                    {
                        Spell = Spell.HornedSorcererDustTornado,
                        Value = damage,
                        ExpireTime = Envir.Time + time + start,
                        TickSpeed = 1000,
                        Direction = Direction,
                        CurrentLocation = new Point(x, y),
                        CastLocation = CurrentLocation,
                        Show = location.X == x && location.Y == y,
                        CurrentMap = CurrentMap,
                        Owner = this,
                        Caster = this
                    };

                    DelayedAction action = new DelayedAction(DelayedType.Spawn, Envir.Time + start, ob);
                    CurrentMap.ActionList.Add(action);
                }
            }
        }

        private void Thrust(MapObject target)
        {
            MirDirection jumpDir = Functions.DirectionFromPoint(CurrentLocation, target.CurrentLocation);

            Point location;

            for (int i = 0; i < 3; i++)
            {
                location = Functions.PointMove(CurrentLocation, jumpDir, 1);
                if (!CurrentMap.ValidPoint(location)) return;
            }

            for (int i = 0; i < 3; i++)
            {
                location = Functions.PointMove(CurrentLocation, jumpDir, 1);

                CurrentMap.GetCell(CurrentLocation).Remove(this);
                RemoveObjects(jumpDir, 1);
                CurrentLocation = location;
                CurrentMap.GetCell(CurrentLocation).Add(this);
                AddObjects(jumpDir, 1);

                int damage = Stats[Stat.MaxDC];

                if (damage > 0)
                {
                    DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 500, location, damage, DefenceType.AC);
                    ActionList.Add(action);
                }
            }

            Broadcast(new S.ObjectDashAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Distance = 3 });
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            Point location = (Point)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            var cell = CurrentMap.GetCell(location);

            if (cell.Objects == null) return;

            for (int o = 0; o < cell.Objects.Count; o++)
            {
                MapObject ob = cell.Objects[o];
                if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) continue;
                if (!ob.IsAttackTarget(this)) continue;

                ob.Attacked(this, damage, defence);
                break;
            }
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool aoe = data.Count >= 4 && (bool)data[3];

            _Immune = false;

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (aoe)
            {
                var targets = FindAllTargets(2, CurrentLocation, false);

                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].Attacked(this, damage, defence);
                }
            }
            else
            {
                target.Attacked(this, damage, defence);
            }
        }
    }
}
