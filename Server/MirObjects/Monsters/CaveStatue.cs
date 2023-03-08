using Server.MirDatabase;

namespace Server.MirObjects.Monsters
{
    public class CaveStatue : MonsterObject
    {
        protected override bool CanMove { get { return false; } }
        protected override bool CanAttack { get { return false; } }
        protected override bool CanRegen { get { return false; } }

        protected internal CaveStatue(MonsterInfo info)
            : base(info)
        {
            if (info.Effect == 1)
            {
                Direction = MirDirection.UpRight;
            }
            else
            {
                Direction = MirDirection.Up;
            }
        }

        public override void Spawned()
        {
            base.Spawned();
        }

        public override void Turn(MirDirection dir) { }

        public override bool Walk(MirDirection dir) { return false; }

        protected override void ProcessRoam() { }

        protected override void ProcessTarget() { }
    }
}
