using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Library.MirDatabase
{
    public class GTMap
    {
        public int Index;
        public int Key;
        public List<Map> Maps = new();
        public string Name;
        public string Owner;
        public string Leader;
        public string Leader2 = string.Empty;
        public int Price;
        public int Days;
        public int Begin;

        public GTMap()
        {
        }

        public GTMap(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Key = reader.ReadInt32();
            Name = reader.ReadString();
            Owner = reader.ReadString();
            Leader = reader.ReadString();
            Leader2 = reader.ReadString();
            Price = reader.ReadInt32();
            Days = reader.ReadInt32();
            Begin = reader.ReadInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Key);
            writer.Write(Name);
            writer.Write(Owner);
            writer.Write(Leader);
            writer.Write(Leader2);
            writer.Write(Price);
            writer.Write(Days);
            writer.Write(Begin);
        }

        public ClientGTMap ToClientGTMap()
        {
            ClientGTMap result = new ClientGTMap
            {
                index = Index,
                Name = Name,
                Owner = Owner,
                Leader = Leader,
                Leader2 = Leader2,
                price = Price,
                days = Days,
                begin = Begin,
            };
            return result;
        }
    }
}
