using Server.MirDatabase;

namespace Server.MirObjects.Monsters
{
    public class CargoBox : MonsterObject
    {
        protected override bool CanMove { get { return false; } }
        protected internal CargoBox(MonsterInfo info)
            : base(info)
        {
        }

        protected override void Attack() { }

        public override void Turn(MirDirection dir) { }

        public override bool Walk(MirDirection dir) { return false; }

        protected override void ProcessRoam() { }

        protected override void ProcessTarget()
        {
        }
    }
}
