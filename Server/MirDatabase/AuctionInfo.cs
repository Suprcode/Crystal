using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Server.MirDatabase.Extensions;

namespace Server.MirDatabase
{
    public class AuctionInfo
    {
        [Key]
        public ulong AuctionID { get; set; }

        public ulong ItemUniqueID { get; set; }
        public UserItem Item;
        public DateTime ConsignmentDate { get; set; }
        public uint Price { get; set; }


        public int CharacterIndex { get; set; }
        public CharacterInfo CharacterInfo;

        public bool Expired { get; set; }
        public bool Sold { get; set; }

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

        public void Save(bool convert = false)
        {
            if (convert) AuctionID = 0;
            Item?.Save(convert);
            using (var accountDb = new AccountDbContext())
            {
                if (AuctionID == 0) accountDb.Auctions.Add(this);
                if (accountDb.Entry(this).State == EntityState.Detached)
                {
                    accountDb.Auctions.Attach(this);
                    accountDb.Characters.Attach(CharacterInfo);
                    accountDb.UserItems.Attach(Item);
                    accountDb.Entry(this).State = EntityState.Modified;
                }

                if (ItemUniqueID != Item?.UniqueID) ItemUniqueID = Item.UniqueID;
                if (CharacterIndex != CharacterInfo.Index) CharacterIndex = CharacterInfo.Index;

                accountDb.SaveChanges();
            }
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
