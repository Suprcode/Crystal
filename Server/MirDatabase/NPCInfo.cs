using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
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
        public string LocationString
        {
            get => $"{Location.X},{Location.Y}";
            set
            {
                var array = value.Split(',');
                Location = array.Length != 2 ? Point.Empty : new Point(int.Parse(array[0]), int.Parse(array[1]));
            }
        }
        public ushort Rate { get; set; } = 100;
        public ushort Image { get; set; }
        [NotMapped]
        public Color Colour { get; set; }

        public int ColorData
        {
            get => Colour.ToArgb();
            set => Colour = Color.FromArgb(value);
        }

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
        public bool IsRobot { get; set; }

        [NotMapped]
        public List<int> CollectQuestIndexes = new List<int>();
        [NotMapped]
        public List<int> FinishQuestIndexes = new List<int>();
        
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

            if (Envir.LoadVersion >= 72)
            {
                Image = reader.ReadUInt16();
            }
            else
            {
                Image = reader.ReadByte();
            }
            
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

        public void Save()
        {
            //TODO: CollectQuestIndexes & FinishQuestIndexes Not Saving, Not Found Usage
            using (Envir.ServerDb = new ServerDbContext())
            {
                if (this.Index == 0) Envir.ServerDb.NpcInfos.Add(this);
                if (Envir.ServerDb.Entry(this).State == EntityState.Detached)
                {
                    Envir.ServerDb.NpcInfos.Attach(this);
                    Envir.ServerDb.Entry(this).State = EntityState.Modified;
                }

                Envir.ServerDb.SaveChanges();
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

            if (!ushort.TryParse(data[5], out var outUShort)) return;
            info.Image = outUShort;
            if (!ushort.TryParse(data[6], out outUShort)) return;
            info.Rate = outUShort;

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
