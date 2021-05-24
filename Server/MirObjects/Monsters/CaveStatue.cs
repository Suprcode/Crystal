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
            if(info.Effect == 1)
            {
                Direction = MirDirection.DownLeft;
                direction = Direction;
            }
            if (info.Effect == 2)
            {
                Direction = MirDirection.DownRight;
                direction = Direction;
            }

        }

        public override void Spawned()
        {
            Direction = direction;

            base.Spawned();
        }

        public override void Turn(MirDirection dir)
        {
        }

        public override bool Walk(MirDirection dir) { return false; }

        protected override void ProcessRoam() { }


        protected override bool InAttackRange()
        {
            return CurrentMap == Target.CurrentMap && Functions.InRange(CurrentLocation, Target.CurrentLocation, Info.ViewRange);
        }
    }
}
