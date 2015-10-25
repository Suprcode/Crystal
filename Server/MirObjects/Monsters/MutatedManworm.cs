using System.Drawing;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class MutatedManworm : MonsterObject
    {
        public long TeleportTime;

        protected internal MutatedManworm(MonsterInfo info)
            : base(info)
        {
        }

        protected override void ProcessTarget()
        {
            if (Target != null && !InAttackRange() && Envir.Time > TeleportTime)
            {
                TeleportTime = Envir.Time + 5000;
                Teleport(CurrentMap, Target.Back, true, 3);
            }
            else
            {
                base.ProcessTarget();
            }
        }

        protected override void Attack()
        {
            if (!Target.IsAttackTarget(this))
            {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            if (Envir.Random.Next(3) > 0)
            {
                base.Attack();
            }
            else
            {
                Broadcast(new S.ObjectAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 1 });
                Attack2();
            }

            ShockTime = 0;
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

        }

        private void Attack2()
        {
            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            Target.Attacked(this, damage, DefenceType.MACAgility);
        }
    }
}
