using Server.Library.MirDatabase;
using Shared;
using Shared.Data;
using Shared.Functions;

namespace Server.Library.MirObjects.Monsters {
    public class FlamingWooma : MonsterObject {
        protected internal FlamingWooma(MonsterInfo info) : base(info) { }

        protected override void Attack() {
            if(!Target.IsAttackTarget(this)) {
                Target = null;
                return;
            }

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            Broadcast(new ServerPacket.ObjectAttack
                { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if(damage == 0) {
                return;
            }

            DelayedAction action = new(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.MACAgility);
            ActionList.Add(action);
        }
    }
}
