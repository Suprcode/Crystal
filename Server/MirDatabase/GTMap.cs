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
        public int index;
        public int key;
        public List<Map> Maps = new();
        public string Name;
        public string Owner;
        public string Leader;
        public string Leader2 = string.Empty;
        public int price;
        public int days;
        public int begin;

        public GTMap()
        {
        }

        public GTMap(BinaryReader reader)
        {
            index = reader.ReadInt32();
            key = reader.ReadInt32();
            Name = reader.ReadString();
            Owner = reader.ReadString();
            Leader = reader.ReadString();
            Leader2 = reader.ReadString();
            price = reader.ReadInt32();
            days = reader.ReadInt32();
            begin = reader.ReadInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(index);
            writer.Write(key);
            writer.Write(Name);
            writer.Write(Owner);
            writer.Write(Leader);
            writer.Write(Leader2);
            writer.Write(price);
            writer.Write(days);
            writer.Write(begin);
        }

        public ClientGTMap ToClientGTMap()
        {
            ClientGTMap result = new ClientGTMap
            {
                index = index,
                Name = Name,
                Owner = Owner,
                Leader = Leader,
                Leader2 = Leader2,
                price = price,
                days = days,
                begin = begin,
            };
            return result;
        }
    }
}
