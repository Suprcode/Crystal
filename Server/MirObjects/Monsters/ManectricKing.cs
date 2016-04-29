using Server.MirDatabase;
using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.Drawing;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class ManectricKing : MonsterObject
    {
        public long FearTime;
        public byte AttackRange = 3;
        private long MassAttackTime;

        protected internal ManectricKing(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;
            if (Target.CurrentLocation == CurrentLocation) return false;

            int x = Math.Abs(Target.CurrentLocation.X - CurrentLocation.X);
            int y = Math.Abs(Target.CurrentLocation.Y - CurrentLocation.Y);

            if (x > AttackRange || y > AttackRange) return false;

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

            if((HP * 100 / MaxHP) < 20 && MassAttackTime < Envir.Time)
            {
                ShockTime = 0;
                ActionTime = Envir.Time + 500;
                AttackTime = Envir.Time + (AttackSpeed);

                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

                List<MapObject> targets = FindAllTargets(7, CurrentLocation, false);

                if (targets.Count == 0) return;

                int damage = GetAttackPower(MinDC, MaxDC);

                for (int i = 0; i < targets.Count; i++)
                {
                    int delay = Functions.MaxDistance(CurrentLocation, targets[i].CurrentLocation) * 50 + 750; //50 MS per Step

                    DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.ACAgility);
                    ActionList.Add(action);
                }

                MassAttackTime = Envir.Time + 2000 + (Envir.Random.Next(5) * 1000);
                ActionTime = Envir.Time + 800;
                AttackTime = Envir.Time + (AttackSpeed);
                return;
            }

            if (Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange - 1) 
                && Envir.Random.Next(3) == 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                LineAttack(AttackRange - Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) + 1, true);
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
                LineAttack(AttackRange);
            }

            ShockTime = 0;
            ActionTime = Envir.Time + 500;
            AttackTime = Envir.Time + (AttackSpeed);
        }

        private void LineAttack(int distance, bool push = false)
        {
            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500;

            for(int i = distance; i >= 1; i--)
            {
                Point target = Functions.PointMove(CurrentLocation, Direction, i);

                if (!CurrentMap.ValidPoint(target)) continue;

                Cell cell = CurrentMap.GetCell(target);
                if (cell.Objects == null) continue;

                for (int o = 0; o < cell.Objects.Count; o++)
                {
                    MapObject ob = cell.Objects[o];
                    if (ob.Race == ObjectType.Monster || ob.Race == ObjectType.Player)
                    {
                        if (!ob.IsAttackTarget(this)) continue;

                        if (push)
                        {
                            ob.Pushed(this, Direction, distance - 1);
                        }

                        ob.Attacked(this, damage, DefenceType.ACAgility);

                    }
                    else continue;

                    break;
                }
            }
        }
    }
}
