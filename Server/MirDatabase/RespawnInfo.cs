using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;
using Server.MirEnvir;

namespace Server.MirDatabase
{
    public class RespawnInfo
    {
        public int id { get; set; }
        public int MonsterIndex { get; set; }
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
        public ushort Count, Spread, Delay, RandomDelay;
        public int DBCount { get { return Count; } set { Count = (ushort) value; } }
        public int DBSpread { get { return Spread; } set { Spread = (ushort) value; } }
        public int DBRandomDelay { get { return RandomDelay;} set { RandomDelay = (ushort) value; } }

        public byte Direction { get; set; }

        public string RoutePath { get; set; } = string.Empty;
        public int RespawnIndex { get; set; }
        public bool SaveRespawnTime { get; set; } = false;
        public ushort RespawnTicks; //leave 0 if not using this system!
        public int DBRespawnTicks { get { return RespawnTicks; } set { RespawnTicks = (ushort) value; } }
        public int MapInfoIndex { get; set; }
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

            int monsterIndex;
            if (!int.TryParse(data[0], out monsterIndex)) return null;
            info.MonsterIndex = monsterIndex;
            if (!int.TryParse(data[1], out x)) return null;
            if (!int.TryParse(data[2], out y)) return null;

            info.Location = new Point(x, y);

            if (!ushort.TryParse(data[3], out info.Count)) return null;
            if (!ushort.TryParse(data[4], out info.Spread)) return null;
            if (!ushort.TryParse(data[5], out info.Delay)) return null;
            byte direction;
            if (!byte.TryParse(data[6], out direction)) return null;
            info.Direction = direction;
            if (!ushort.TryParse(data[7], out info.RandomDelay)) return null;
            int respawnIndex;
            if (!int.TryParse(data[8], out respawnIndex)) return null;
            info.RespawnIndex = respawnIndex;
            bool saveRespawnTime;
            if (!bool.TryParse(data[9], out saveRespawnTime)) return null;
            info.SaveRespawnTime = saveRespawnTime;
            if (!ushort.TryParse(data[10], out info.RespawnTicks)) return null;

            return info;
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