using Server.Library.MirDatabase;
using Shared;

namespace Server.Library.MirObjects.Monsters {
    public class CaveStatue : MonsterObject {
        protected override bool CanMove => false;
        protected override bool CanAttack => false;
        protected override bool CanRegen => false;

        protected internal CaveStatue(MonsterInfo info)
            : base(info) {
            if(info.Effect == 1) {
                Direction = MirDirection.UpRight;
            } else {
                Direction = MirDirection.Up;
            }
        }

        public override void Spawned() {
            base.Spawned();
        }

        public override void Turn(MirDirection dir) { }

        public override bool Walk(MirDirection dir) {
            return false;
        }

        protected override void ProcessRoam() { }

        protected override void ProcessTarget() { }
    }
}
