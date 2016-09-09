using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Server.MirEnvir;

namespace Server.MirDatabase
{
    public class NPCInfo
    {
        [Key]
        public int Index { get; set; }

        public string FileName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public int MapIndex { get; set; }
        [NotMapped]
        public Point Location;
        public string DBLocation
        {
            get { return Location.X + "," + Location.Y; }
            set
            {
                var tempArray = value.Split(',');
                if (tempArray.Length != 2)
                {
                    Location.X = 0;
                    Location.Y = 0;
                }
                else
                {
                    int result = 0;
                    int.TryParse(tempArray[0], out result);
                    Location.X = result;
                    int.TryParse(tempArray[1], out result);
                    Location.Y = result;
                }
            }
        }
        public ushort Rate = 100;
        public int DBRate { get { return Rate; }set { Rate = (ushort) value; } }

        public byte Image { get; set; }

        public bool TimeVisible { get; set; } = false;
        public byte HourStart { get; set; } = 0;
        public byte MinuteStart { get; set; } = 0;
        public byte HourEnd { get; set; } = 0;
        public byte MinuteEnd { get; set; } = 1;
        public short MinLev { get; set; } = 0;
        public short MaxLev { get; set; } = 0;
        public string DayofWeek { get; set; } = "";
        public string ClassRequired { get; set; } = "";
        public bool Sabuk { get; set; } = false;
        public int FlagNeeded { get; set; } = 0;
        public int Conquest { get; set; }

        public bool IsDefault { get; set; }
        [NotMapped]
        public List<int> CollectQuestIndexes = new List<int>();

        public string DBCollectQuestIndexes
        {
            get { return string.Join(",", CollectQuestIndexes); }
            set { CollectQuestIndexes = string.IsNullOrEmpty(value) ? new List<int>() : value.Split(',').Select(int.Parse).ToList(); }
        }
        [NotMapped]
        public List<int> FinishQuestIndexes = new List<int>();

        public string DBFinishQuestIndexes
        {
            get { return string.Join(",", FinishQuestIndexes); }
            set { FinishQuestIndexes = string.IsNullOrEmpty(value) ? new List<int>() : value.Split(',').Select(int.Parse).ToList(); }
        }
        
        public NPCInfo()
        { }
        public NPCInfo(BinaryReader reader)
        {
            if (Envir.LoadVersion > 33)
            {
                Index = reader.ReadInt32();
                MapIndex = reader.ReadInt32();

                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                    CollectQuestIndexes.Add(reader.ReadInt32());

                count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                    FinishQuestIndexes.Add(reader.ReadInt32());
            }

            FileName = reader.ReadString();
            Name = reader.ReadString();

            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Image = reader.ReadByte();
            
            Rate = reader.ReadUInt16();

            if (Envir.LoadVersion >= 64)
            {
                TimeVisible = reader.ReadBoolean();
                HourStart = reader.ReadByte();
                MinuteStart = reader.ReadByte();
                HourEnd = reader.ReadByte();
                MinuteEnd = reader.ReadByte();
                MinLev = reader.ReadInt16();
                MaxLev = reader.ReadInt16();
                DayofWeek = reader.ReadString();
                ClassRequired = reader.ReadString();
                if (Envir.LoadVersion >= 66)
                    Conquest = reader.ReadInt32();
                else
                    Sabuk = reader.ReadBoolean();
                FlagNeeded = reader.ReadInt32();
            }
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(MapIndex);

            writer.Write(CollectQuestIndexes.Count());
            for (int i = 0; i < CollectQuestIndexes.Count; i++)
                writer.Write(CollectQuestIndexes[i]);

            writer.Write(FinishQuestIndexes.Count());
            for (int i = 0; i < FinishQuestIndexes.Count; i++)
                writer.Write(FinishQuestIndexes[i]);

            writer.Write(FileName);
            writer.Write(Name);

            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Image);
            writer.Write(Rate);

            writer.Write(TimeVisible);
            writer.Write(HourStart);
            writer.Write(MinuteStart);
            writer.Write(HourEnd);
            writer.Write(MinuteEnd);
            writer.Write(MinLev);
            writer.Write(MaxLev);
            writer.Write(DayofWeek);
            writer.Write(ClassRequired);
            writer.Write(Conquest);
            writer.Write(FlagNeeded);
        }

        public static void FromText(string text)
        {
            string[] data = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length < 6) return;

            NPCInfo info = new NPCInfo { Name = data[0] };

            int x, y;

            info.FileName = data[0];
            info.MapIndex = SMain.EditEnvir.MapInfoList.Where(d => d.FileName == data[1]).FirstOrDefault().Index;

            if (!int.TryParse(data[2], out x)) return;
            if (!int.TryParse(data[3], out y)) return;

            info.Location = new Point(x, y);

            info.Name = data[4];

            byte result;
            if (!byte.TryParse(data[5], out result)) return;
            info.Image = result;
            if (!ushort.TryParse(data[6], out info.Rate)) return;

            info.Index = ++SMain.EditEnvir.NPCIndex;
            SMain.EditEnvir.NPCInfoList.Add(info);
        }
        public string ToText()
        {
            return string.Format("{0},{1},{2},{3},{4},{5},{6}",
                FileName, SMain.EditEnvir.MapInfoList.Where(d => d.Index == MapIndex).FirstOrDefault().FileName, Location.X, Location.Y, Name, Image, Rate);
        }

        public override string ToString()
        {
            return string.Format("{0}:   {1}", FileName, Functions.PointToString(Location));
        }
    }
}
