using Server.MirDatabase;
using Server.MirEnvir;
using System;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class HellKnight : MonsterObject
    {
        public bool Summoned;
        public HellLord Lord;

        protected internal HellKnight(MonsterInfo info)
            : base(info)
        {
        }

        public override void Spawned()
        {
            base.Spawned();

            Summoned = true;
        }

        public override void Die()
        {
            if(Lord != null)
            {
                Lord.KnightKilled();
            }

            base.Die();
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
