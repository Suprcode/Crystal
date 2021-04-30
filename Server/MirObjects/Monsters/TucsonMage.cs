using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class TucsonMage : MonsterObject
    {
        protected internal TucsonMage(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > 5 || y > 5) return false;


            return (x <= 4 && y <= 4) || (x == y || x % 5 == y % 5);
        }

        protected override void Attack()
        {

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            bool range = !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if (!range && Envir.Random.Next(5) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
                if (damage == 0) return;
                Target.Attacked(this, damage);
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                LineAttack(2);
            }


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;


        }

        private void LineAttack(int distance)
        {

            int damage = GetAttackPower(Stats[Stat.MinMC], Stats[Stat.MaxMC]);
            if (damage == 0) return;

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
    }
}
