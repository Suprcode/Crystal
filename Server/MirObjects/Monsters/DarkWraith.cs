using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class DarkWraith : MonsterObject
    {
        private const byte AttackRange = 4;
        public long LineAttackTime;

        protected internal DarkWraith(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);
        }

        protected bool InRangeAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected override void ProcessTarget()
        {
            if (Target == null || Dead) return;

            if (!InAttackRange())
            {
                if (CanAttack)
                {
                    if (Envir.Random.Next(3) == 0)
                        LineAttack(4);
                }
                if (CurrentLocation == Target.CurrentLocation)
                {
                    MirDirection direction = (MirDirection)Envir.Random.Next(8);
                    int rotation = Envir.Random.Next(2) == 0 ? 1 : -1;

                    for (int d = 0; d < 8; d++)
                    {
                        if (Walk(direction)) break;

                        direction = Functions.ShiftDirection(direction, rotation);
                    }
                }
                else
                    MoveTo(Target.CurrentLocation);
            }

            if (!CanAttack) return;

            if (Envir.Random.Next(3) > 0)
            {
                if (InAttackRange())
                    Attack();
            }
            else LineAttack(4);

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }
        }

        protected override void Attack()
        {

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if (damage == 0) return;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            List<MapObject> targets = FindAllTargets(1, CurrentLocation);

            if (targets.Count > 1 && Envir.Random.Next(2) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                for (int i = 0; i < targets.Count; i++)
                {
                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 600, targets[i], damage, DefenceType.AC);
                    ActionList.Add(action);
                }
            }
            else
            {
                base.Attack();
            }


            ActionTime = Envir.Time + 500;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;


        }

        private void LineAttack(int distance)
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            if (LineAttackTime > Envir.Time) return;
            ShockTime = 0;
            ActionTime = Envir.Time + 500;
            AttackTime = Envir.Time + AttackSpeed + 500;
            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]) * 3;
            if (damage == 0) return;

            LineAttackTime = Envir.Time + 3000 + Envir.Random.Next(5) * 1000;
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2 });

            for (int i = 1; i <= distance; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (target == Target.CurrentLocation)
                {
                    Target.Attacked(this, damage, DefenceType.ACAgility);
                }
                else
                {
                    if (!CurrentMap.ValidPoint(target)) continue;

                    Cell cell = CurrentMap.GetCell(target);
                    if (cell.Objects == null) continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player)
                        {
                            if (!ob.IsAttackTarget(this)) continue;

                            ob.Attacked(this, damage, DefenceType.AC);
                        }
                        else continue;

                        break;
                    }

                }
            }
        }
    }
}