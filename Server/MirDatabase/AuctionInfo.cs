using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Server.MirDatabase
{
    public class AuctionInfo
    {
        public ulong AuctionID; 

        public UserItem Item;
        public DateTime ConsignmentDate;
        public uint Price;

        public int CharacterIndex;
        public CharacterInfo CharacterInfo;

        public bool Expired, Sold;

        public AuctionInfo()
        {
            
        }
        public AuctionInfo(BinaryReader reader, int version, int customversion)
        {
            AuctionID = reader.ReadUInt64();

            Item = new UserItem(reader, version, customversion);
            ConsignmentDate = DateTime.FromBinary(reader.ReadInt64());
            Price = reader.ReadUInt32();

            CharacterIndex = reader.ReadInt32();

            Expired = reader.ReadBoolean();
            Sold = reader.ReadBoolean();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(AuctionID);

            Item.Save(writer);
            writer.Write(ConsignmentDate.ToBinary());
            writer.Write(Price);

            writer.Write(CharacterIndex);

            writer.Write(Expired);
            writer.Write(Sold);

        }

        public ClientAuction CreateClientAuction(bool userMatch)
        {
            return new ClientAuction
                {
                    AuctionID = AuctionID,
                    Item = Item,
                    Seller = userMatch ? (Sold ? "Sold" : (Expired ? "Expired" : "For Sale")) : CharacterInfo.Name,
                    Price = Price,
                    ConsignmentDate = ConsignmentDate,
                };
        }
    }
}
