using Server.MirDatabase;
using Server.MirEnvir;
using System.Collections.Generic;
using System.Drawing;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class FlameQueen : MonsterObject
    {
        public long FearTime;
        public byte AttackRange = 7;
        private long MassAttackTime;

        protected internal FlameQueen(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
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
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });
            }

            ShockTime = 0;
            ActionTime = Envir.Time + 500;
            AttackTime = Envir.Time + (AttackSpeed);
        }
        
    }
}
