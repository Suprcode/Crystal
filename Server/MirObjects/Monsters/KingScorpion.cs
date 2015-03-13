using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class KingScorpion : MonsterObject
    {
        protected internal KingScorpion(MonsterInfo info) : base(info)
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

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Point target = Functions.PointMove(CurrentLocation, Direction, 2);

            bool range = false;

            if (CurrentMap.ValidPoint(target))
            {
                Cell cell = CurrentMap.GetCell(target);
                if (cell.Objects != null)
                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race != ObjectType.Monster && ob.Race != ObjectType.Player) continue;
                        if (!ob.IsAttackTarget(this)) continue;
                        range = true;
                        break;
                    }
            }


            if (range || Envir.Random.Next(5) == 0)
                Broadcast(new S.ObjectRangeAttack {ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation});
            else
                Broadcast(new S.ObjectAttack {ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation});

            LineAttack(2, range);


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
            ShockTime = 0;
        }

        private void LineAttack(int distance, bool range)
        {
            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;
            
            for (int i = 1; i <= distance; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (target == Target.CurrentLocation)
                    Target.Attacked(this, damage, range ? DefenceType.MACAgility : DefenceType.ACAgility);
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
                            ob.Attacked(this, damage, range ? DefenceType.MACAgility : DefenceType.ACAgility);
                        }
                        else continue;

                        break;
                    }
                }
            }
        }
    }
}
