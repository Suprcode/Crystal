using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class DarkOmaKing : MonsterObject
    {
        private long _OrbTime;
        private long _MassThunderTime;

        protected virtual byte AttackRange
        {
            get
            {
                return 6;
            }
        }

        protected internal DarkOmaKing(MonsterInfo info) 
            : base(info)
        {
            _MassThunderTime = Envir.Time + 10000;
            _OrbTime = Envir.Time + 20000;
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 3);

            if (Envir.Time > _OrbTime)
            {
                _OrbTime = Envir.Time + 20000;

                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });

                ActionTime = Envir.Time + AttackSpeed + 300;

                var attempts = 4;
                var distance = 8;
                var count = 2;

                for (int j = 0; j < count; j++)
                {
                    for (int i = 0; i < attempts; i++)
                    {
                        var location = new Point(CurrentLocation.X + Envir.Random.Next(-distance, distance + 1),
                                             CurrentLocation.Y + Envir.Random.Next(-distance, distance + 1));

                        if (location == CurrentLocation || location == Target.CurrentLocation) continue;

                        if (PowerBead.SpawnRandom(this, location)) break;
                    }
                }    
            }

            if (Envir.Time > _MassThunderTime)
            {
                _MassThunderTime = Envir.Time + 10000 + Envir.Random.Next(0, 5000);

                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 3 });

                ActionTime = Envir.Time + AttackSpeed + 300;

                int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                if (damage == 0) return;

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility, true);
                ActionList.Add(action);

                return;
            }

            if (!ranged)
            {
                if (Envir.Random.Next(4) > 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                    ActionTime = Envir.Time + AttackSpeed + 300;

                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility);
                    ActionList.Add(action);
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                    ActionTime = Envir.Time + AttackSpeed + 3400;

                    int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                    if (damage == 0) return;

                    FullmoonAttack(damage, 500, DefenceType.ACAgility, 1, 2);
                    FullmoonAttack(damage, 1700, DefenceType.ACAgility, 1, 2);
                    FullmoonAttack(damage, 2500, DefenceType.ACAgility, 1, 2);

                    var start = 3000;

                    SpellObject ob = new SpellObject
                    {
                        Spell = Spell.DarkOmaKingNuke,
                        Value = Stats[Stat.MaxDC],
                        ExpireTime = Envir.Time + 900 + start,
                        TickSpeed = 1000,
                        Direction = Direction,
                        CurrentLocation = Functions.PointMove(CurrentLocation, Direction, 3),
                        CastLocation = CurrentLocation,
                        Show = true,
                        CurrentMap = CurrentMap,
                        Caster = this
                    };

                    DelayedAction action2 = new DelayedAction(DelayedType.Spawn, Envir.Time + start, ob);
                    CurrentMap.ActionList.Add(action2);
                }
            }
            else
            {
                if (Envir.Random.Next(3) == 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 0 });

                    ActionTime = Envir.Time + AttackSpeed + 300;

                    int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
                    if (damage == 0) return;

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.MAC);
                    ActionList.Add(action);
                }
            }

            ShockTime = 0;
            AttackTime = Envir.Time + AttackSpeed;
        }

        protected override void CompleteAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];
            bool aoe = data.Count >= 4 && (bool)data[3];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            if (aoe)
            {
                var targets = FindAllTargets(5, CurrentLocation, false);

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

        public override void Die()
        {
            base.Die();

            //Kill Minions
            for (int i = SlaveList.Count - 1; i >= 0; i--)
            {
                if (!SlaveList[i].Dead && SlaveList[i].Node != null)
                {
                    SlaveList[i].Die();
                }
            }
        }
    }
}
