using System;
using System.Collections.Generic;
using System.IO;


namespace Server.MirDatabase
{
    public class PublicEventInfo
    {
        public string MultipleCoords = string.Empty;
        public short EventSize = 0;
        public string EventName = string.Empty;
        public int CooldownInMinutes = 0;
        public List<EventRespawn> Respawns = new List<EventRespawn>();
        public MapInfo Info;
        public int Index = 0;

        public string ObjectiveMessage = string.Empty;

        public EventType EventType = EventType.Invasion;
        public bool IsSafezone = false;

        public PublicEventInfo()
        {

        }
        public PublicEventInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            MultipleCoords = reader.ReadString();
            EventSize = reader.ReadInt16();
            EventName = reader.ReadString();
            CooldownInMinutes = reader.ReadInt32();

            sbyte type = reader.ReadSByte();
            EventType = (EventType)type;

            IsSafezone = reader.ReadBoolean();

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
                Respawns.Add(new EventRespawn(reader));

            if (MirEnvir.Envir.LoadCustomVersion >= 2)
                ObjectiveMessage = reader.ReadString();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(MultipleCoords);
            writer.Write(EventSize);
            writer.Write(EventName);
            writer.Write(CooldownInMinutes);
            writer.Write((sbyte)EventType);
            writer.Write(IsSafezone);

            writer.Write(Respawns.Count);
            for (int i = 0; i < Respawns.Count; i++)
                Respawns[i].Save(writer);

            writer.Write(ObjectiveMessage);
        }
        public override string ToString()
        {
            return string.Format("[{0}] {1}", Index, EventName);
        }

    }

    public class EventRespawn
    {
        public string MonsterName;
        public ushort MonsterCount;
        public ushort Order;
        public ushort SpreadX;
        public ushort SpreadY;
        public bool IsObjective = false;
        public EventRespawn()
        {
            MonsterName = string.Empty;
            MonsterCount = 0;
            Order = 0;
            SpreadX = 0;
            SpreadY = 0;
            IsObjective = false;
        }
        public EventRespawn(BinaryReader reader)
        {
            MonsterName = reader.ReadString();
            MonsterCount = reader.ReadUInt16();
            Order = reader.ReadUInt16();
            SpreadX = reader.ReadUInt16();
            SpreadY = reader.ReadUInt16();
            IsObjective = reader.ReadBoolean();
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(MonsterName);
            writer.Write(MonsterCount);
            writer.Write(Order);
            writer.Write(SpreadX);
            writer.Write(SpreadY);
            writer.Write(IsObjective);
        }
        public override string ToString()
        {
            return string.Format("Monster: {0} - Count: {1} ", MonsterName, MonsterCount.ToString());
        }
    }
}