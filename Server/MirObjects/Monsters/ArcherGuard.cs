using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class ArcherGuard : Guard
    {
        protected internal ArcherGuard(MonsterInfo info)
            : base(info)
        {

        }

        protected override bool CanMove
        {
            get { return false; }
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
            Broadcast(new S.ObjectRangeAttack { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID });

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);

            if (Target.Race != ObjectType.Player) damage = int.MaxValue;

            if (damage == 0) return;

            ProjectileAttack(damage);
        }
    }
}
