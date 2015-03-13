using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class MinotaurKing : RightGuard
    {
        protected override byte AttackRange
        {
            get
            {
                return 6;
            }
        }

        protected internal MinotaurKing(MonsterInfo info)
            : base(info)
        {
        }

        protected override void ProcessTarget()
        {
            if (Target == null || !CanAttack) return;

            if (InAttackRange())
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
