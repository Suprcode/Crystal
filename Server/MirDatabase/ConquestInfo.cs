using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Server.MirDatabase
{
    public class ConquestInfo
    {
        [Key]
        public int Index { get; set; }
        public bool FullMap { get; set; }
        [NotMapped]
        public Point Location;

        public string LocationString
        {
            get => $"{Location.X},{Location.Y}";
            set
            {
                var array = value.Split(',');
                Location = array.Length != 2 ? Point.Empty : new Point(int.Parse(array[0]), int.Parse(array[1]));
            }
        }
        public ushort Size { get; set; }
        public string Name { get; set; }
        public int MapIndex { get; set; }
        public int PalaceIndex { get; set; }
        [NotMapped]
        public List<int> ExtraMaps = new List<int>();
        [NotMapped]
        public List<ConquestArcherInfo> ConquestGuards = new List<ConquestArcherInfo>();
        [NotMapped]
        public List<ConquestGateInfo> ConquestGates = new List<ConquestGateInfo>();
        [NotMapped]
        public List<ConquestWallInfo> ConquestWalls = new List<ConquestWallInfo>();
        [NotMapped]
        public List<ConquestSiegeInfo> ConquestSieges = new List<ConquestSiegeInfo>();
        [NotMapped]
        public List<ConquestFlagInfo> ConquestFlags = new List<ConquestFlagInfo>();

        public int GuardIndex { get; set; }
        public int GateIndex { get; set; }
        public int WallIndex { get; set; }
        public int SiegeIndex { get; set; }
        public int FlagIndex { get; set; }

        public byte StartHour { get; set; } = 0;
        public int WarLength { get; set; } = 60;

        private int counter { get; set; }

        public ConquestType Type { get; set; } = ConquestType.Request;
        public ConquestGame Game { get; set; } = ConquestGame.CapturePalace;

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        //King of the hill
        public string KingLocationString
        {
            get => $"{KingLocation.X},{KingLocation.Y}";
            set
            {
                var array = value.Split(',');
                KingLocation = array.Length != 2 ? Point.Empty : new Point(int.Parse(array[0]), int.Parse(array[1]));
            }
        }
        [NotMapped]
        public Point KingLocation;
        public ushort KingSize { get; set; }

        //Control points
        [NotMapped]
        public List<ConquestFlagInfo> ControlPoints = new List<ConquestFlagInfo>();
        public int ControlPointIndex { get; set; }

        public byte[] BinData { get; set; }

        public ConquestInfo()
        {

        }

        public void Load()
        {
            using (var ms = new MemoryStream(BinData))
            using (var reader = new BinaryReader(ms))
            {
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
                counter = reader.ReadInt32();
                for (int i = 0; i < counter; i++)
                {
                    ConquestFlags.Add(new ConquestFlagInfo(reader));
                }

                counter = reader.ReadInt32();
                for (int i = 0; i < counter; i++)
                {
                    ControlPoints.Add(new ConquestFlagInfo(reader));
                }
            }
        }

        public ConquestInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();

            if(Envir.LoadVersion > 73)
            {
                FullMap = reader.ReadBoolean();
            }

            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Size = reader.ReadUInt16();
            Name = reader.ReadString();
            MapIndex = reader.ReadInt32();
            PalaceIndex = reader.ReadInt32();
            GuardIndex = reader.ReadInt32();
            GateIndex = reader.ReadInt32();
            WallIndex = reader.ReadInt32();
            SiegeIndex = reader.ReadInt32();

            if (Envir.LoadVersion > 72)
            {
                FlagIndex = reader.ReadInt32();
            }

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

            if (Envir.LoadVersion > 72)
            {
                counter = reader.ReadInt32();
                for (int i = 0; i < counter; i++)
                {
                    ConquestFlags.Add(new ConquestFlagInfo(reader));
                }
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

            KingLocation = new Point(reader.ReadInt32(), reader.ReadInt32());
            KingSize = reader.ReadUInt16();

            if (Envir.LoadVersion > 74)
            {
                ControlPointIndex = reader.ReadInt32();
                counter = reader.ReadInt32();
                for (int i = 0; i < counter; i++)
                {
                    ControlPoints.Add(new ConquestFlagInfo(reader));
                }
            }
        }

        public void Save()
        {
            //TODO:Use Config File To Save This
            using (Envir.ServerDb = new ServerDbContext())
            {
                if (this.Index == 0) Envir.ServerDb.ConquestInfos.Add(this);
                if (Envir.ServerDb.Entry(this).State == EntityState.Detached)
                {
                    Envir.ServerDb.ConquestInfos.Attach(this);
                    Envir.ServerDb.Entry(this).State = EntityState.Modified;
                }

                using (var ms = new MemoryStream())
                using (var writer = new BinaryWriter(ms))
                {
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

                    writer.Write(ConquestFlags.Count);
                    for (int i = 0; i < ConquestFlags.Count; i++)
                    {
                        ConquestFlags[i].Save(writer);
                    }
                    for (int i = 0; i < ControlPoints.Count; i++)
                    {
                        ControlPoints[i].Save(writer);
                    }

                    BinData = ms.ToArray();
                }
                Envir.ServerDb.SaveChanges();
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(FullMap);
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
            writer.Write(FlagIndex);

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

            writer.Write(ConquestFlags.Count);
            for (int i = 0; i < ConquestFlags.Count; i++)
            {
                ConquestFlags[i].Save(writer);
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

            writer.Write(KingLocation.X);
            writer.Write(KingLocation.Y);
            writer.Write(KingSize);

            writer.Write(ControlPointIndex);
            writer.Write(ControlPoints.Count);
            for (int i = 0; i < ControlPoints.Count; i++)
            {
                ControlPoints[i].Save(writer);
            }

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

    public class ConquestFlagInfo
    {
        public int Index;
        public Point Location;
        public string Name;
        public string FileName = string.Empty;

        public ConquestFlagInfo()
        {

        }

        public ConquestFlagInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Name = reader.ReadString();
            FileName = reader.ReadString();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Name);
            writer.Write(FileName);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", Index, Name, Location);
        }
    }
}
