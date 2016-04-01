using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class HellSlasher : MonsterObject
    {
        protected internal HellSlasher(MonsterInfo info)
            : base(info)
        {
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }
            
            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            if (Envir.Random.Next(3) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

                MirDirection dir = Functions.PreviousDir(Direction);
                Point target;
                Cell cell;

                for (int i = 0; i < 4; i++)
                {
                    target = Functions.PointMove(CurrentLocation, Direction, 1);
                    dir = Functions.NextDir(Direction);
                    if (target == Front) continue;

                    if (!CurrentMap.ValidPoint(target)) continue;

                    cell = CurrentMap.GetCell(target);

                    if (cell.Objects == null) continue;

                    for (int o = 0; o < cell.Objects.Count; o++)
                    {
                        MapObject ob = cell.Objects[o];
                        if (ob.Race != ObjectType.Player && ob.Race != ObjectType.Monster) continue;
                        if (!ob.IsAttackTarget(this)) continue;

                        ob.Attacked(this, MinDC, DefenceType.ACAgility);

                        if (Envir.Random.Next(Settings.PoisonResistWeight) >= ob.PoisonResist)
                        {
                            if (Envir.Random.Next(5) == 0)
                            {
                                ob.ApplyPoison(new Poison { PType = PoisonType.Stun, Duration = Envir.Random.Next(1, 4), TickSpeed = 1000 }, this);
                            }
                        }
                                      
                        break;
                    }
                }
            }

            ShockTime = 0;
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            Target.Attacked(this, damage, DefenceType.ACAgility);
        }   
    }
}
