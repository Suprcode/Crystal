using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class StoningStatue : MonsterObject
    {
        private const byte AttackRange = 6;

        protected internal StoningStatue(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > 2 || y > 2) return false;


            return (x <= 1 && y <= 1) || (x == y || x % 2 == y % 2);
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


            if (!ranged)
            {
                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;

                Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                LineAttack(2);

            }
            else
            {
                if (Envir.Random.Next(5) == 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

                    ActionTime = Envir.Time + 300;
                    AttackTime = Envir.Time + AttackSpeed;

                    int damage = GetAttackPower(MinMC, MaxMC);
                    if (damage == 0) return;

                    int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.MACAgility);
                    ActionList.Add(action);
                }
                else
                {
                    MoveTo(Target.CurrentLocation);
                }
            }


            if (Target.Dead)
                FindTarget();

        }
        private void LineAttack(int distance)
        {

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;
            
            int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500;

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

                            ob.Attacked(this, damage, DefenceType.ACAgility);
                        }
                        else continue;

                        break;
                    }

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

            MoveTo(Target.CurrentLocation);

        }
    }
}
