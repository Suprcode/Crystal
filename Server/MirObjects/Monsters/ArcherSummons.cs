using System.Collections.Generic;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class ArcherSummons : MonsterObject
    {
        public bool Summoned;

        protected override bool CanMove { get { if(this.Info.AI == 59) return false; else return true; } }

        protected internal ArcherSummons(MonsterInfo info)
            : base(info)
        {
        }

        public override void Spawned()
        {
            base.Spawned();

            Summoned = true;
        }

        public override void Turn(MirDirection dir)
        {
        }

        public override bool Walk(MirDirection dir) { return false; }

        protected override void ProcessRoam() { }
        
        protected override void CompleteAttack(IList<object> data)
        {

        }
        protected override void ProcessTarget()
        {

        }

        public override Packet GetInfo()
        {
            return new S.ObjectMonster
            {
                ObjectID = ObjectID,
                Name = Name,
                NameColour = NameColour,
                Location = CurrentLocation,
                Image = Info.Image,
                Direction = Direction,
                Effect = Info.Effect,
                AI = Info.AI,
                Light = Info.Light,
                Dead = Dead,
                Skeleton = Harvested,
                Poison = CurrentPoison,
                Hidden = Hidden,
                Extra = Summoned,
            };
        }
    }
}
