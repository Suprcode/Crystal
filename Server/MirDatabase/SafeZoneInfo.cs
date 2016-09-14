using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Server.MirDatabase
{
    public class SafeZoneInfo
    {
        public int id { get; set; }
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

        public ushort Size;
        public int DBSize { get { return Size; } set { Size = (ushort) value; } }

        public bool StartPoint { get; set; }

        public int MapInfoIndex { get; set; }
        [NotMapped]
        public MapInfo Info;

        public SafeZoneInfo()
        {

        }

        public SafeZoneInfo(BinaryReader reader)
        {
            Location = new Point(reader.ReadInt32(), reader.ReadInt32());
            Size = reader.ReadUInt16();
            StartPoint = reader.ReadBoolean();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Location.X);
            writer.Write(Location.Y);
            writer.Write(Size);
            writer.Write(StartPoint);
        }

        public override string ToString()
        {
            return string.Format("Map: {0}- {1}", Functions.PointToString(Location), StartPoint);
        }
    }
}
