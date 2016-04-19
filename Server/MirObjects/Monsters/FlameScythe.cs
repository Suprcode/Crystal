using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Collections.Generic;

namespace Server.MirObjects.Monsters
{
    class FlameScythe : MonsterObject
    {
        public long FearTime;
        public byte AttackRange = 2;

        protected internal FlameScythe(MonsterInfo info)
            : base(info)
        {
        }

        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, AttackRange);
        }

        protected override void Attack()
        {
            ShockTime = 0;

            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            if(Functions.InRange(Target.CurrentLocation, CurrentLocation, 1))
            {
                base.Attack();
            }
            else
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

                List<MapObject> targets = FindAllTargets(2, Target.CurrentLocation, false);

                int damage = GetAttackPower(MinMC, MaxMC);

                if (damage > 0 && targets.Count > 0)
                {
                    for (int i = 0; i < targets.Count; i++)
                    {
                        if (Envir.Random.Next(Settings.MagicResistWeight) >= targets[i].MagicResist)
                        {
                            DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, targets[i], damage, DefenceType.MACAgility);
                            ActionList.Add(action);
                        }
                    }
                }
            }

            AttackTime = Envir.Time + AttackSpeed;
            ActionTime = Envir.Time + 300;
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
    }
}
