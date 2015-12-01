using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Server.MirDatabase
{
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
}
