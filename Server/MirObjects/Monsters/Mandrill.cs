using Server.Library.MirDatabase;
using Shared;
using Shared.Data;
using Shared.Functions;

namespace Server.Library.MirObjects.Monsters {
    public class Mandrill : MutatedManworm {
        public override byte TeleportEffect => 7;

        protected internal Mandrill(MonsterInfo info)
            : base(info) { }

        protected override void Attack() {
            if(!Target.IsAttackTarget(this)) {
                Target = null;
                return;
            }

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);

            Broadcast(new ServerPacket.ObjectAttack
                { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, Type = 0 });

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if(damage == 0) {
                return;
            }

            DelayedAction action = new(DelayedType.Damage, Envir.Time + 300, Target, damage, DefenceType.ACAgility);
            ActionList.Add(action);

            ShockTime = 0;
            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;
        }
    }
}
