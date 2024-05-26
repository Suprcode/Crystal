using Server.Library.MirDatabase;
using Shared;
using Shared.Data;
using Shared.Functions;

namespace Server.Library.MirObjects.Monsters {
    public class FrozenZombie : MonsterObject {
        protected internal FrozenZombie(MonsterInfo info)
            : base(info) { }

        protected override bool InAttackRange() {
            return CurrentMap == Target.CurrentMap &&
                   Functions.InRange(CurrentLocation, Target.CurrentLocation, Info.ViewRange);
        }

        protected override void Attack() {
            if(!Target.IsAttackTarget(this)) {
                Target = null;
                return;
            }

            ShockTime = 0;

            Direction = Functions.DirectionFromPoint(CurrentLocation, Target.CurrentLocation);
            bool ranged = CurrentLocation == Target.CurrentLocation ||
                          !Functions.InRange(CurrentLocation, Target.CurrentLocation, 1);

            ActionTime = Envir.Time + 300;
            AttackTime = Envir.Time + AttackSpeed;

            int damage = GetAttackPower(Stats[Stat.MinDC], Stats[Stat.MaxDC]);
            if(damage == 0) {
                return;
            }

            if(!ranged) {
                Broadcast(new ServerPacket.ObjectAttack
                    { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });

                DelayedAction action = new(DelayedType.Damage, Envir.Time + 300, Target, damage,
                    DefenceType.MACAgility);
                ActionList.Add(action);
            } else {
                Broadcast(new ServerPacket.ObjectRangeAttack {
                    ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation, TargetID = Target.ObjectID
                });
                AttackTime = Envir.Time + AttackSpeed + 500;

                DelayedAction action = new(DelayedType.RangeDamage, Envir.Time + 500, Target, damage, DefenceType.MAC);
                ActionList.Add(action);
            }
        }

        protected override void ProcessTarget() {
            if(Target == null) {
                return;
            }

            if(InAttackRange() && CanAttack) {
                Attack();
                return;
            }

            if(Envir.Time < ShockTime) {
                Target = null;
                return;
            }

            MoveTo(Target.CurrentLocation);
        }
    }
}
