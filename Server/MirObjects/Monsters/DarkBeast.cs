using System;
using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class DarkBeast : MonsterObject
    {
        protected internal DarkBeast(MonsterInfo info)
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

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            if (Envir.Random.Next(5) > 0)
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                int damage = GetAttackPower(MinDC, MaxDC);
                if (damage == 0) return;
                Target.Attacked(this, damage, DefenceType.ACAgility);
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                Attack2();
            }
        }

        private void Attack2()
        {
            int damage = GetAttackPower(MinDC, MaxDC * 2);
            if (damage == 0) return;
            Target.Attacked(this, damage, DefenceType.ACAgility);

            if (Envir.Random.Next(5) == 0)
                if (Envir.Random.Next(Settings.PoisonResistWeight) >= Target.PoisonResist)
                {
                    Target.ApplyPoison(new Poison { PType = PoisonType.Bleeding, Duration = 5, TickSpeed = 1000 }, this);
                }
        }

    }
}
