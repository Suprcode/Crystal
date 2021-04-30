using System.Collections.Generic;
using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;
using System.Linq;
using System.Text;

namespace Server.MirObjects.Monsters
{
    class SeedingsGeneral : MonsterObject
    {
        protected virtual byte AttackRange
        {
            get
            {
                return 12;
            }
        }

        protected internal SeedingsGeneral(MonsterInfo info)
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
            bool ranged = CurrentLocation == Target.CurrentLocation || !Functions.InRange(CurrentLocation, Target.CurrentLocation, 2);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            if (!ranged && Envir.Random.Next(5) > 0)
            {
                if (Envir.Random.Next(5) > 0)
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                    LineAttack1(1); //Blood Attack
                }
                else
                {
                    Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                    LineAttack2(1);//Green Splash Attack
                }

            }
            else
            {
                if (Envir.Random.Next(5) > 0)
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID, Type = 1 });
                    RangeAttack2(2);//Blue Fireball Projectile
                }
                else
                {
                    Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });
                    RangeAttack1(2);//Echo Shout
                }

            }


            if (Target.Dead)
                FindTarget();

        }

        private void LineAttack1(int distance)
        {
            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;            
            Target.Attacked(this, damage, DefenceType.ACAgility);
        }

        private void LineAttack2(int distance)
        {
            int damage = GetAttackPower(MinDC, MaxDC * 2);
            if (damage == 0) return;            
            Target.Attacked(this, damage, DefenceType.ACAgility);
        }

        private void RangeAttack1(int distance)//Echo Shout
        {
            int damage = GetAttackPower(MinMC, MaxMC);
            if (damage == 0) return;
            Target.Attacked(this, damage, DefenceType.MACAgility);

            if (Envir.Random.Next(5) == 0)
                Target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Frozen, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 1000 }, this);

            ActionTime = Envir.Time + 600;
            AttackTime = Envir.Time + AttackSpeed;
        }

        private void RangeAttack2(int distance)//Blue Fireball Projectile
        {
            int damage = GetAttackPower(MinMC, MaxMC * 2);
            if (damage == 0) return;
            Target.Attacked(this, damage, DefenceType.MACAgility);

            if (Envir.Random.Next(5) == 0)
                Target.ApplyPoison(new Poison { Owner = this, Duration = 5, PType = PoisonType.Slow, Value = GetAttackPower(MinSC, MaxSC), TickSpeed = 1000 }, this);

            ActionTime = Envir.Time + 600;
            AttackTime = Envir.Time + AttackSpeed;
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

