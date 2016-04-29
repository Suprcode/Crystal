using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class DarkDevourer : MonsterObject
    {
        private const byte AttackRange = 9;

        protected internal DarkDevourer(MonsterInfo info)
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
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            if (!ranged)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

                ActionTime = Envir.Time + AttackSpeed + 300;

                int damage = GetAttackPower(MinDC, MaxDC);
                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.AC);
                ActionList.Add(action);
            }
            else
            {
                Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

                AttackTime = Envir.Time + AttackSpeed + 500;

                int damage = GetAttackPower(MinSC, MaxSC);
                DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MACAgility);
                ActionList.Add(action);

                if(Info.Effect == 1)
                {
                    if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                    {
                        Target.ApplyPoison(new Poison
                        {
                            Owner = this,
                            Duration = 5,
                            Value = damage,
                            PType = PoisonType.Green,
                            TickSpeed = 1000,
                        }, this);
                    }
                }
            }

            ShockTime = 0;
            AttackTime = Envir.Time + AttackSpeed;

            if (Target.Dead)
                FindTarget();
        }

        protected override void ProcessTarget()
        {
            if (Target == null) return;

            if (InAttackRange() && CanAttack)
            {
                Attack();
                return;
            }

            if (Envir.Time < ShockTime)
            {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);

        }
    }
}
