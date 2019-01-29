using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;
using Server.MirEnvir;

namespace Server.MirDatabase
{
    public class RespawnInfo
    {
        [Key]
        public int Index { get; set; }
        public int MonsterIndex { get; set; }

        public string LocationString
        {
            get => $"{Location.X},{Location.Y}";
            set
            {
                var array = value.Split(',');
                Location = array.Length != 2 ? Point.Empty : new Point(int.Parse(array[0]), int.Parse(array[1]));
            }
        }
        [NotMapped]
        public Point Location;
        public ushort Count { get; set; }
        public ushort Spread { get; set; }
        public ushort Delay { get; set; }
        public ushort RandomDelay { get; set; }
        public byte Direction { get; set; }

        public string RoutePath { get; set; } = string.Empty;
        public int RespawnIndex { get; set; }
        public bool SaveRespawnTime { get; set; } = false;
        public ushort RespawnTicks { get; set; } //leave 0 if not using this system!

        public int MapIndex { get; set; }

        public RespawnInfo()
        {

        }
        public RespawnInfo(BinaryReader reader, int Version, int Customversion)
        {
            MonsterIndex = reader.ReadInt32();

            Location = new Point(reader.ReadInt32(), reader.ReadInt32());

            Count = reader.ReadUInt16();
            Spread = reader.ReadUInt16();

            Delay = reader.ReadUInt16();
            Direction = reader.ReadByte();

            if (Envir.LoadVersion >= 36)
            {
                RoutePath = reader.ReadString();
            }

            if (Version > 67)
            {
                RandomDelay = reader.ReadUInt16();
                RespawnIndex = reader.ReadInt32();
                SaveRespawnTime = reader.ReadBoolean();
                RespawnTicks = reader.ReadUInt16();
            }
            else
            {
                RespawnIndex = ++SMain.Envir.RespawnIndex;
            }
        }

        public static RespawnInfo FromText(string text)
        {
            string[] data = text.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length < 7) return null;

            RespawnInfo info = new RespawnInfo();

            int x,y ;

            if (!int.TryParse(data[0], out var outInt)) return null;
            info.MonsterIndex = outInt;
            if (!int.TryParse(data[1], out outInt)) return null;
            x = outInt;
            if (!int.TryParse(data[2], out outInt)) return null;
            y = outInt;

            info.Location = new Point(x, y);

            if (!ushort.TryParse(data[3], out var outUShort)) return null;
            info.Count = outUShort;
            if (!ushort.TryParse(data[4], out outUShort)) return null;
            info.Spread = outUShort;
            if (!ushort.TryParse(data[5], out outUShort)) return null;
            info.Delay = outUShort;
            if (!byte.TryParse(data[6], out var outByte)) return null;
            info.Direction = outByte;
            if (!ushort.TryParse(data[7], out outUShort)) return null;
            info.RandomDelay = outUShort;
            if (!int.TryParse(data[8], out outInt)) return null;
            info.RespawnIndex = outInt;
            if (!bool.TryParse(data[9], out var outBool)) return null;
            info.SaveRespawnTime = outBool;
            if (!ushort.TryParse(data[10], out outUShort)) return null;
            info.RespawnTicks = outUShort;

            return info;
        }

        public void Save()
        {
            Envir.ServerDb.Respawns.Add(this);
            Envir.ServerDb.SaveChanges();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(MonsterIndex);

            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Count);
            writer.Write(Spread);

            writer.Write(Delay);
            writer.Write(Direction);

            writer.Write(RoutePath);

            writer.Write(RandomDelay);
            writer.Write(RespawnIndex);
            writer.Write(SaveRespawnTime);
            writer.Write(RespawnTicks);
        }

        public override string ToString()
        {
            return string.Format("Monster: {0} - {1} - {2} - {3} - {4} - {5} - {6} - {7} - {8} - {9}", MonsterIndex, Functions.PointToString(Location), Count, Spread, Delay, Direction, RandomDelay, RespawnIndex, SaveRespawnTime, RespawnTicks);
        }

        
    }

    public class RouteInfo
    {
        public Point Location;
        public int Delay;

        public static RouteInfo FromText(string text)
        {
            string[] data = text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length < 2) return null;

            RouteInfo info = new RouteInfo();

            int x, y;

            if (!int.TryParse(data[0], out x)) return null;
            if (!int.TryParse(data[1], out y)) return null;

            info.Location = new Point(x, y);

            if (data.Length <= 2) return info;

            return !int.TryParse(data[2], out info.Delay) ? info : info;
        }
    }
}