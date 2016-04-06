using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class WitchDoctor : MonsterObject
    {
        public long FearTime;
        public byte AttackRange = 6;

        protected internal WitchDoctor(MonsterInfo info)
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

            ShockTime = 0;

            if (Envir.Random.Next(5) == 0)
            {
                TeleportRandom(40, AttackRange);
            }
            else
            {
                Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;

                int damage = GetAttackPower(MinMC, MaxMC);
                if (damage == 0) return;

                int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.MACAgility);
                ActionList.Add(action);

                if (Target.Dead)
                    FindTarget();
            }
        }

        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;

            if (InAttackRange() && Envir.Time < FearTime)
            {
                Attack();
                return;
            }

            FearTime = Envir.Time + 5000;

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            int dist = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation);

            if (dist >= AttackRange)
                MoveTo(Target.CurrentLocation);
            else
            {
                MirDirection dir = Functions.DirectionFromPoint(Target.CurrentLocation, CurrentLocation);

                if (Walk(dir)) return;

                switch (Envir.Random.Next(2)) //No favour
                {
                    case 0:
                        for (int i = 0; i < 7; i++)
                        {
                            dir = Functions.NextDir(dir);

                            if (Walk(dir))
                                return;
                        }
                        break;
                    default:
                        for (int i = 0; i < 7; i++)
                        {
                            dir = Functions.PreviousDir(dir);

                            if (Walk(dir))
                                return;
                        }
                        break;
                }
                
            }
        }

        public override bool TeleportRandom(int attempts, int distance, Map temp = null)
        {
            if (Target == null) return false;

            for (int i = 0; i < attempts; i++)
            {
                Point location;

                location = new Point(Target.CurrentLocation.X + Envir.Random.Next(-distance, distance + 1),
                                          Target.CurrentLocation.Y + Envir.Random.Next(-distance, distance + 1));

                if (Teleport(CurrentMap, location, true, 5)) return true;
            }

            return false;
        }
    }
}
