using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    class BoneFamiliar : MonsterObject
    {
        public bool Summoned;

        protected internal BoneFamiliar(MonsterInfo info) : base(info)
        {
            Direction = MirDirection.DownLeft;
        }
        
        public override void Spawned()
        {
            base.Spawned();

            Summoned = true;
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
