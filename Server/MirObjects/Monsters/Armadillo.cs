using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class Armadillo : DigOutZombie
    {
        protected bool _runAway;

        protected internal Armadillo(MonsterInfo info)
            : base(info)
        {
        }

        protected override void SpawnDigOutEffect()
        {
            if (Visible && Envir.Time > DigOutTime + 500 && !DoneDigOut)
            {
                SpellObject ob = new SpellObject
                {
                    Spell = Spell.DigOutArmadillo,
                    Value = 1,
                    ExpireTime = Envir.Time + (5 * 60 * 1000),
                    TickSpeed = 2000,
                    Caster = null,
                    CurrentLocation = DigOutLocation,
                    CurrentMap = this.CurrentMap,
                    Direction = DigOutDirection
                };
                CurrentMap.AddObject(ob);
                ob.Spawned();
                DoneDigOut = true;
            }
        }

        public override int Attacked(HumanObject attacker, int damage, DefenceType type = DefenceType.ACAgility, bool damageWeapon = true)
        {
            if (_runAway && Envir.Random.Next(4) == 0)
            {
                _runAway = false;
            }

            return base.Attacked(attacker, damage, type, damageWeapon);
        }

        public override int Attacked(MonsterObject attacker, int damage, DefenceType type = DefenceType.ACAgility)
        {
            if (_runAway && Envir.Random.Next(4) == 0)
            {
                _runAway = false;
            }

            return base.Attacked(attacker, damage, type);
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

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            switch (Envir.Random.Next(0, 6))
            {
                case 0:
                    {
                        Retreat();
                    }
                    break;
                case 1:
                    {
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                        int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                        if (damage == 0) return;

                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 400, Target, damage / 2, DefenceType.ACAgility);
                        ActionList.Add(action);

                        damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                        if (damage == 0) return;

                        action = new DelayedAction(DelayedType.Damage, Envir.Time + 600, Target, damage / 2, DefenceType.ACAgility);
                        ActionList.Add(action);

                        damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                        if (damage == 0) return;

                        action = new DelayedAction(DelayedType.Damage, Envir.Time + 800, Target, damage / 2, DefenceType.ACAgility);
                        ActionList.Add(action);
                    }
                    break;
                default:
                    {
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                        int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                        if (damage == 0) return;

                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 400, Target, damage, DefenceType.ACAgility);
                        ActionList.Add(action);
                    }
                    break;
            }
        }

        protected void Retreat()
        {
            MirDirection jumpDir = Functions.ReverseDirection(Direction);

            Point location = new Point();

            for (int i = 0; i < 2; i++)
            {
                location = Functions.PointMove(CurrentLocation, jumpDir, 1);
                if (!CurrentMap.ValidPoint(location)) return;
            }

            for (int i = 0; i < 2; i++)
            {
                location = Functions.PointMove(CurrentLocation, jumpDir, 1);

                CurrentMap.GetCell(CurrentLocation).Remove(this);
                RemoveObjects(jumpDir, 1);
                CurrentLocation = location;
                CurrentMap.GetCell(CurrentLocation).Add(this);
                AddObjects(jumpDir, 1);
            }

            Broadcast(new S.ObjectBackStep { ObjectID = ObjectID, Direction = Direction, Location = location, Distance = 2 });

            int damage = Stats[Stat.MaxDC];
            if (damage == 0) return;

            DelayedAction action = new DelayedAction(DelayedType.RangeDamage, Envir.Time + 900, Target, damage, DefenceType.AC);
            ActionList.Add(action);
        }

        protected override void CompleteRangeAttack(IList<object> data)
        {
            MapObject target = (MapObject)data[0];
            int damage = (int)data[1];
            DefenceType defence = (DefenceType)data[2];

            if (target == null || !target.IsAttackTarget(this) || target.CurrentMap != CurrentMap || target.Node == null) return;

            var targets = FindAllTargets(2, CurrentLocation);

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].Attacked(this, damage, defence) <= 0)
                {
                    _runAway = true;
                };
            }
        }

        protected override void ProcessTarget()
        {
            if (_runAway)
            {
                if (!CanMove || Target == null) return;

                MirDirection dir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);

                if (Walk(dir)) return;

                switch (Envir.Random.Next(2)) //No favour
                {
                    case 0:
                        for (int i = 0; i < 7; i++)
                        {
                            dir = Functions.NextDir(dir);

                            if (Walk(dir))
                                return;
                        }
                        break;
                    default:
                        for (int i = 0; i < 7; i++)
                        {
                            dir = Functions.PreviousDir(dir);

                            if (Walk(dir))
                                return;
                        }
                        break;
                }
            }
            else base.ProcessTarget();
        }
    }
}
