using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class Behemoth : MonsterObject
    {
        public byte AttackRange = 5;

        protected internal Behemoth(MonsterInfo info)
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

            if (!ranged)
            {
                if (Envir.Random.Next(2) > 0)
                {
                    base.Attack();
                }
                else
                {
                    if (Envir.Random.Next(2) > 0)
                    {
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1});
                        Attack1();
                    }
                    else
                    {
                        Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 2});
                        Attack2();
                    }
                }

                int damage = GetAttackPower(MinDC, MaxDC);
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation,});
                if (damage == 0) return;

                Target.Attacked(this, damage, DefenceType.ACAgility);

                ShockTime = 0;
                ActionTime = Envir.Time + 300;
                AttackTime = Envir.Time + AttackSpeed;
                if (Envir.Random.Next(15) == 0)
                    {
                        Target.ApplyPoison(new Poison { Owner = this, Duration = 15, PType = PoisonType.Bleeding, TickSpeed = 1000 }, this);
                    }

            }
            else
            {
                if (Envir.Random.Next(2) == 0)
                {
                    MoveTo(Target.CurrentLocation);
                }
                else
                {
                    if (Envir.Random.Next(2) > 0)
                    {
                        Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
                        int damage = GetAttackPower(MinMC, MaxMC);
                        if (damage == 0) return;

                        ActionTime = Envir.Time + 300;
                        AttackTime = Envir.Time + AttackSpeed;

                        int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.MACAgility);
                        ActionList.Add(action);
                    }
                    else
                    {
                        Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });
                        int damage = GetAttackPower(MinMC, MaxMC);
                        if (damage == 0) return;

                        ActionTime = Envir.Time + 300;
                        AttackTime = Envir.Time + AttackSpeed;

                        int delay = Functions.MaxDistance(CurrentLocation, Target.CurrentLocation) * 50 + 500; //50 MS per Step

                        DelayedAction action = new DelayedAction(DelayedType.Damage, Envir.Time + delay, Target, damage, DefenceType.MACAgility);
                        ActionList.Add(action);
                    }
                }
            }
        }
        private void Attack1()
        {
            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            Target.Attacked(this, damage, DefenceType.ACAgility);
        }
        private void Attack2()
        {
            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            Target.Attacked(this, damage, DefenceType.ACAgility);

            if (Envir.Random.Next(8) == 0)
            {
                Target.ApplyPoison(new Poison { Owner = this, Duration = 3, PType = PoisonType.Stun, TickSpeed = 1000 }, this);
            }
        }

    }
}