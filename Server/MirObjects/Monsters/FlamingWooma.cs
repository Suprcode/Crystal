using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class FlamingWooma : MonsterObject
    {
        protected internal FlamingWooma(MonsterInfo info) : base(info)
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
            Broadcast(new S.ObjectAttack {ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation});


            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(MinDC, MaxDC);
            if (damage == 0) return;

            Target.Attacked(this, damage, DefenceType.MACAgility);
        }
    }
}
