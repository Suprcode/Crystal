using System;
using System.Collections.Generic;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class CaveStatue : MonsterObject
    {

        MirDirection direction;

        protected override bool CanMove { get { return false; } }

        protected internal CaveStatue(MonsterInfo info)
            : base(info)
        {
            if(info.Effect == 0)
            {
                Direction = MirDirection.Up;
                direction = Direction;
            }
            if (info.Effect == 1)
            {
                Direction = MirDirection.UpRight;
                direction = Direction;
            }
        }

        public override void Spawned()
        {
            base.Spawned();
        }

        public override void Turn(MirDirection dir) { }

        public override bool Walk(MirDirection dir) { return false; }

        protected override void ProcessRoam() { }

        protected override void ProcessTarget()
        {
            
        }
    }
}
