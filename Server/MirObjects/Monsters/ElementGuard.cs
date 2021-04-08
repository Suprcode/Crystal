﻿using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class ElementGuard : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 8;
            }
        }

        protected internal ElementGuard(MonsterInfo info)
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

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            if (!ranged)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                int damage = GetAttackPower(MinDC, MaxDC);
                if (damage == 0) return;
                Target.Attacked(this, damage, DefenceType.ACAgility);
            }
            else
            {
                {
                    if (Envir.Random.Next(3) > 0)
                    {
                        //Fire Attack
                        Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
                        AttackTime = Envir.Time + AttackSpeed + 500;
                        int damage = GetAttackPower(MinMC, MaxMC);
                        if (damage == 0) return;

                        if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                        {
                            if (Envir.Random.Next(5) == 0)
                                Target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Red, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 1000 }, this);
                        }

                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MAC);
                        ActionList.Add(action);

                    }
                    else
                    {
                        //Fire 2 Attack
                        Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });
                        AttackTime = Envir.Time + AttackSpeed + 500;
                        int damage = GetAttackPower(MinMC, MaxMC);
                        if (damage == 0) return;

                        if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                        {
                            if (Envir.Random.Next(5) == 0)
                                Target.ApplyPoison(new Poison { Owner = this, Duration = 3, PType = PoisonType.Green, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 1000 }, this);
                        }

                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + 500, Target, damage, DefenceType.MAC);
                        ActionList.Add(action);
                    }
                }
            }


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
