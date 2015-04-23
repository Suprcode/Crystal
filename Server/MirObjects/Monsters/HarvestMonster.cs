using System;
using System.Collections.Generic;
using System.Linq;
using Server.MirDatabase;
using S = ServerPackets;

namespace Server.MirObjects.Monsters
{
    public class HarvestMonster : MonsterObject
    {
        protected short Quality;
        protected int RemainingSkinCount;

        private List<UserItem> _drops;


        protected internal HarvestMonster(MonsterInfo info)
            : base(info)
        {
            RemainingSkinCount = 2;
        }

        protected override void Drop() { }
        public override bool Harvest(PlayerObject player)
        {
            if (RemainingSkinCount == 0)
            {
                for (int i = _drops.Count - 1; i >= 0; i--)
                {
                    if (player.CheckGroupQuestItem(_drops[i]))
                    {
                        _drops.RemoveAt(i); 
                    }
                    else
                    {
                        if (player.CanGainItem(_drops[i]))
                        {
                            player.GainItem(_drops[i]);
                            _drops.RemoveAt(i);
                        }
                    }
                }

                if (_drops.Count == 0)
                {
                    Harvested = true;
                    _drops = null;
                    Broadcast(new S.ObjectHarvested { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
                }
                else player.ReceiveChat("You cannot carry anymore.", ChatType.System);

                return true;
            }

            if (--RemainingSkinCount > 0) return true;


            _drops = new List<UserItem>();

            for (int i = 0; i < Info.Drops.Count; i++)
            {
                DropInfo drop = Info.Drops[i];

                int rate = (int)(drop.Chance / Settings.DropRate); if (rate < 1) rate = 1;
                if (drop.Gold > 0 || Envir.Random.Next(rate) != 0) continue;

                UserItem item = Envir.CreateDropItem(drop.Item);
                if (item == null) continue;

                if (drop.QuestRequired)
                {
                    if (!player.CheckGroupQuestItem(item, false)) continue;
                }

                if (item.Info.Type == ItemType.Meat)
                    item.CurrentDura = (ushort)Math.Max(0, item.CurrentDura + Quality);

                _drops.Add(item);
            }


            if (_drops.Count == 0)
            {
                player.ReceiveChat("Nothing was found.", ChatType.System);
                Harvested = true;
                _drops = null;
                Broadcast(new S.ObjectHarvested { ObjectID = ObjectID, Direction = Direction, Location = CurrentLocation });
            }


            return true;
        }

        /*
        public override bool MagAttacked(MonsterObject A, int Damage)
        {
            bool B = base.MagAttacked(A, Damage);

            if (B)
                Quality = (short)Math.Max(short.MinValue, Quality - 2000);
            return true;
        }*/
    }
}
