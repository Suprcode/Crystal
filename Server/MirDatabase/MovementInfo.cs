using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Server.MirEnvir;

namespace Server.MirDatabase
{
    public class MovementInfo
    {
        [Key]
        public int Index { get; set; }
        public int MapIndex { get; set; }
        public Point Source;

        public string SourceString
        {
            get => $"{Source.X},{Source.Y}";
            set
            {
                var array = value.Split(',');
                Source = array.Length != 2 ? Point.Empty : new Point(int.Parse(array[0]), int.Parse(array[1]));
            }
        }
        public Point Destination;
        public string DestinationString
        {
            get => $"{Destination.X},{Destination.Y}";
            set
            {
                var array = value.Split(',');
                Destination = array.Length != 2 ? Point.Empty : new Point(int.Parse(array[0]), int.Parse(array[1]));
            }
        }
        public bool NeedHole { get; set; }
        public bool NeedMove { get; set; }
        public int ConquestIndex { get; set; }

        public MovementInfo()
        {

        }

        public MovementInfo(BinaryReader reader)
        {
            MapIndex = reader.ReadInt32();
            Source = new Point(reader.ReadInt32(), reader.ReadInt32());
            Destination = new Point(reader.ReadInt32(), reader.ReadInt32());

            if (Envir.LoadVersion < 16) return;
            NeedHole = reader.ReadBoolean();

            if (Envir.LoadVersion < 48) return;
            NeedMove = reader.ReadBoolean();

            if (Envir.LoadVersion < 69) return;
            ConquestIndex = reader.ReadInt32();
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(MapIndex);
            writer.Write(Source.X);
            writer.Write(Source.Y);
            writer.Write(Destination.X);
            writer.Write(Destination.Y);
            writer.Write(NeedHole);
            writer.Write(NeedMove);
            writer.Write(ConquestIndex);
        }

        public void Save()
        {
            Envir.ServerDb.Movements.Add(this);
            Envir.ServerDb.SaveChanges();
        }


        public override string ToString()
        {
            return string.Format("{0} -> Map :{1} - {2}", Source, MapIndex, Destination);
        }
    }
}
