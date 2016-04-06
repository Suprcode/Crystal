using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class CrystalSpider : MonsterObject
    {
        protected internal CrystalSpider(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > 3 || y > 3) return false;


            return (x == 0) || (y == 0) || (x == y);
        }

        protected override void Attack()
        {

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if (!ranged) base.Attack();
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                LineAttack(3);

                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;
                ShockTime = 0;
            }
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
                    if (Envir.Random.Next(Settings.MagicResistWeight) >= Target.MagicResist)
                    {
                        if (Target.Attacked(this, damage, DefenceType.MACAgility) > 0 && Envir.Random.Next(8) == 0)
                        {
                            if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                            {
                                int poison = GetAttackPower(MinSC, MaxSC);

                                Target.ApplyPoison(new Poison
                                {
                                    Owner = this,
                                    Duration = 5,
                                    PType = PoisonType.Green,
                                    Value = poison,
                                    TickSpeed = 2000
                                }, this);
                            }
                        }
                    }
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
                            if (Envir.Random.Next(Settings.MagicResistWeight) < ob.MagicResist) continue;
                            if (ob.Attacked(this, damage, DefenceType.MACAgility) > 0 && Envir.Random.Next(8) == 0)
                            {
                                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                                {
                                    int poison = GetAttackPower(MinSC, MaxSC);

                                    ob.ApplyPoison(new Poison
                                    {
                                        Owner = this,
                                        Duration = 5,
                                        PType = PoisonType.Green,
                                        Value = poison,
                                        TickSpeed = 2000
                                    }, this);
                                }
                            }
                        }
                        else continue;

                        break;
                    }

                }
            }
        }
    }
}
