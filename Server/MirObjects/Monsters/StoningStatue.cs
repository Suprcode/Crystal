using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    public class StoningStatue : MonsterObject
    {
        private long _areaTime = long.MaxValue;

        private const byte AttackRange = 2;

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

            if(_areaTime == long.MaxValue)
            {
                _areaTime = Envir.Time + 10000;
            }

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            if (Envir.Time < _areaTime)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
                LineAttack(AttackRange);

                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;

                if (Target.Dead)
                    FindTarget();

                return;
            }

            _areaTime = Envir.Time + 5000 + Envir.Random.Next(10) * 1000;

            Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });

            ActionList.Add(new DelayedAction(DelayedType.Damage, Envir.Time + 1600));

            ActionTime = Envir.Time + 500;
            AttackTime = Envir.Time + (AttackSpeed * 2);
        }

        protected override void CompleteAttack(IList<object> data)
        {
            List<MapObject> targets = FindAllTargets(1, Functions.PointMove(CurrentLocation, Direction, 2), false);
            if (targets.Count == 0) return;

            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].Attacked(this, MaxDC * 3, DefenceType.MACAgility);

                if (Envir.Random.Next(2) == 0)
                {
                    if (Envir.Random.Next(Settings.PoisonResistWeight) >= targets[i].PoisonResist)
                    {
                        int poisonLength = GetAttackPower(MinMC, MaxMC);

                        targets[i].ApplyPoison(new Poison { Owner = this, PType = PoisonType.Stun, Duration = poisonLength, TickSpeed = 1000 }, this);
                        Broadcast(new S.ObjectEffect { ObjectID = targets[i].ObjectID, Effect = SpellEffect.Stunned, Time = (uint)poisonLength * 1000 });
                    }
                }
            }
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
    }
}
