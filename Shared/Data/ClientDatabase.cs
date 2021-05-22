using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shared
{
    public class ClientDatabaseResponse
    {
        public bool RecievedItems;
        public bool RecievedQuests;
        public bool RecievedGameShopItems;

        public ClientDatabaseResponse() { }
        public ClientDatabaseResponse(BinaryReader reader)
        {
            RecievedItems = reader.ReadBoolean();
            RecievedQuests = reader.ReadBoolean();
            RecievedGameShopItems = reader.ReadBoolean();
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(RecievedItems);
            writer.Write(RecievedQuests);
            writer.Write(RecievedGameShopItems);
        }
    }

    public class ClientDatabase
    {
        public int DataVersion;
        public int CustomVersion;
        public List<ItemInfo> ItemInfoList = new List<ItemInfo>();
        public List<ClientQuestInfo> QuestInfoList = new List<ClientQuestInfo>();
        public List<GameShopItem> GameShopInfoList = new List<GameShopItem>();

        public void Read(BinaryReader reader)
        {
            DataVersion = reader.ReadInt32();
            CustomVersion = reader.ReadInt32();

            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                ItemInfoList.Add(new ItemInfo(reader));
            }

            count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                QuestInfoList.Add(new ClientQuestInfo(reader));
            }

            count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                GameShopInfoList.Add(new GameShopItem(reader, false));
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(DataVersion);
            writer.Write(CustomVersion);

            writer.Write(ItemInfoList.Count);
            for (var i = 0; i < ItemInfoList.Count; i++)
                ItemInfoList[i].Save(writer);

            writer.Write(QuestInfoList.Count);
            for (var i = 0; i < QuestInfoList.Count; i++)
                QuestInfoList[i].Save(writer);

            writer.Write(GameShopInfoList.Count);
            for (var i = 0; i < GameShopInfoList.Count; i++)
                GameShopInfoList[i].Save(writer, false);
        }
    }
}
