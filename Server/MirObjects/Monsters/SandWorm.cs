using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class SandWorm : SpittingSpider
    {
        protected internal SandWorm(MonsterInfo info)
            : base(info)
        {
        }

        private void LineAttack(int distance)
        {

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            for (int i = 1; i <= distance; i++)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (target == Target.CurrentLocation)
                {
                    Target.Attacked(this, damage, DefenceType.MACAgility);
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

                            ob.Attacked(this, damage, DefenceType.MACAgility);
                        }
                        else continue;

                        break;
                    }

                }
            }
        }
    }
}
