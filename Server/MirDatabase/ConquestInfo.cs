using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Server.MirDatabase
{
    public class ConquestInfo
    {
        public int Index;
        public Point Location;
        public ushort Size;
        public string Name;
        public int MapIndex;
        public int PalaceIndex;
        public List<int> ExtraMaps = new List<int>();
        public List<ConquestArcherInfo> ConquestGuards = new List<ConquestArcherInfo>();
        public List<ConquestGateInfo> ConquestGates = new List<ConquestGateInfo>();
        public List<ConquestWallInfo> ConquestWalls = new List<ConquestWallInfo>();
        public List<ConquestSiegeInfo> ConquestSieges = new List<ConquestSiegeInfo>();
        public int GuardIndex;
        public int GateIndex;
        public int WallIndex;
        public int SiegeIndex;

        public byte StartHour = 0;
        public int WarLength = 60;

        private int counter;

        public ConquestType Type = ConquestType.Request;
        public ConquestGame Game = ConquestGame.CapturePalace;

        public bool Monday;
        public bool Tuesday;
        public bool Wednesday;
        public bool Thursday;
        public bool Friday;
        public bool Saturday;
        public bool Sunday;

        public Point ObjectLoc;
        public ushort ObjectSize;

        public ConquestInfo()
        {

        }

        public ConquestInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Size = reader.ReadUInt16();
            Name = reader.ReadString();
            MapIndex = reader.ReadInt32();
            PalaceIndex = reader.ReadInt32();
            GuardIndex = reader.ReadInt32();
            GateIndex = reader.ReadInt32();
            WallIndex = reader.ReadInt32();
            SiegeIndex = reader.ReadInt32();
            counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                ConquestGuards.Add(new ConquestArcherInfo(reader));
            }
            counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                ExtraMaps.Add(reader.ReadInt32());
            }
            counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                ConquestGates.Add(new ConquestGateInfo(reader));
            }
            counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                ConquestWalls.Add(new ConquestWallInfo(reader));
            }
            counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                ConquestSieges.Add(new ConquestSiegeInfo(reader));
            }
            StartHour = reader.ReadByte();
            WarLength = reader.ReadInt32();
            Type = (ConquestType)reader.ReadByte();
            Game = (ConquestGame)reader.ReadByte();

            Monday = reader.ReadBoolean();
            Tuesday = reader.ReadBoolean();
            Wednesday = reader.ReadBoolean();
            Thursday = reader.ReadBoolean();
            Friday = reader.ReadBoolean();
            Saturday = reader.ReadBoolean();
            Sunday = reader.ReadBoolean();

            ObjectLoc = new Point(reader.ReadInt32(), reader.ReadInt32());
            ObjectSize = reader.ReadUInt16();

        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Size);
            writer.Write(Name);
            writer.Write(MapIndex);
            writer.Write(PalaceIndex);
            writer.Write(GuardIndex);
            writer.Write(GateIndex);
            writer.Write(WallIndex);
            writer.Write(SiegeIndex);
            writer.Write(ConquestGuards.Count);
            for (int i = 0; i < ConquestGuards.Count; i++)
            {
                ConquestGuards[i].Save(writer);
            }
            writer.Write(ExtraMaps.Count);
            for (int i = 0; i < ExtraMaps.Count; i++)
            {
                writer.Write(ExtraMaps[i]);
            }
            writer.Write(ConquestGates.Count);
            for (int i = 0; i < ConquestGates.Count; i++)
            {
                ConquestGates[i].Save(writer);
            }
            writer.Write(ConquestWalls.Count);
            for (int i = 0; i < ConquestWalls.Count; i++)
            {
                ConquestWalls[i].Save(writer);
            }
            writer.Write(ConquestSieges.Count);
            for (int i = 0; i < ConquestSieges.Count; i++)
            {
                ConquestSieges[i].Save(writer);
            }
            writer.Write(StartHour);
            writer.Write(WarLength);
            writer.Write((byte)Type);
            writer.Write((byte)Game);

            writer.Write(Monday);
            writer.Write(Tuesday);
            writer.Write(Wednesday);
            writer.Write(Thursday);
            writer.Write(Friday);
            writer.Write(Saturday);
            writer.Write(Sunday);

            writer.Write(ObjectLoc.X);
            writer.Write(ObjectLoc.Y);
            writer.Write(ObjectSize);



        }

        public override string ToString()
        {
            return string.Format("{0}- {1}", Index, Name);
        }
    }

    public class ConquestSiegeInfo
    {
        public int Index;
        public Point Location;
        public int MobIndex;
        public string Name;
        public uint RepairCost;

        public ConquestSiegeInfo()
        {

        }

        public ConquestSiegeInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            MobIndex = reader.ReadInt32();
            Name = reader.ReadString();
            RepairCost = reader.ReadUInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(MobIndex);
            writer.Write(Name);
            writer.Write(RepairCost);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", Index, Name, Location);
        }


    }

    public class ConquestWallInfo
    {
        public int Index;
        public Point Location;
        public int MobIndex;
        public string Name;
        public uint RepairCost;

        public ConquestWallInfo()
        {

        }

        public ConquestWallInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            MobIndex = reader.ReadInt32();
            Name = reader.ReadString();
            RepairCost = reader.ReadUInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(MobIndex);
            writer.Write(Name);
            writer.Write(RepairCost);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", Index, Name, Location);
        }


    }

    public class ConquestGateInfo
    {
        public int Index;
        public Point Location;
        public int MobIndex;
        public string Name;
        public uint RepairCost;

        public ConquestGateInfo()
        {

        }

        public ConquestGateInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            MobIndex = reader.ReadInt32();
            Name = reader.ReadString();
            RepairCost = reader.ReadUInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(MobIndex);
            writer.Write(Name);
            writer.Write(RepairCost);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", Index, Name, Location);
        }


    }

    public class ConquestArcherInfo
    {
        public int Index;
        public Point Location;
        public int MobIndex;
        public string Name;
        public uint RepairCost;

        public ConquestArcherInfo()
        {

        }

        public ConquestArcherInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            MobIndex = reader.ReadInt32();
            Name = reader.ReadString();
            RepairCost = reader.ReadUInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(MobIndex);
            writer.Write(Name);
            writer.Write(RepairCost);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", Index, Name, Location);
        }


    }
}
