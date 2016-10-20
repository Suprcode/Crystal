using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;

namespace Server.MirDatabase
{
    public class AuctionInfo
    {
        [Key]
        public long AuctionID { get; set; }
        [ForeignKey("Item")]
        public long UserItemUniqueID { get; set; }
        public UserItem Item { get; set; }
        public DateTime ConsignmentDate { get; set; }
        public uint Price;
        public long DBPrice { get { return Price; } set { Price = (uint) value; } }
        [ForeignKey("CharacterInfo")]
        public int CharacterIndex { get; set; }
        public CharacterInfo CharacterInfo { get; set; }

        public bool Expired { get; set; }
        public bool Sold { get; set; }

        public AuctionInfo()
        {
            
        }
        public AuctionInfo(BinaryReader reader, int version, int customversion)
        {
            AuctionID = (long) reader.ReadUInt64();

            Item = new UserItem(reader, version, customversion);
            ConsignmentDate = DateTime.FromBinary(reader.ReadInt64());
            Price = reader.ReadUInt32();

            CharacterIndex = reader.ReadInt32();

            Expired = reader.ReadBoolean();
            Sold = reader.ReadBoolean();
        }

        public void Save(BinaryWriter writer)
        {
            if (Settings.UseSQLServer)
            {
                using (var ctx = new DataContext())
                {
                    ctx.UserItems.AddOrUpdate(i => new { i.UniqueID }, Item);
                    var info = this;
                    info.UserItemUniqueID = Item.UniqueID;
                    info.CharacterIndex = CharacterInfo.Index;
                    info.Item = null;
                    info.CharacterInfo = null;
                    var dbInfo = ctx.AuctionInfos.FirstOrDefault(i => i.AuctionID == AuctionID);
                    if (dbInfo == null)
                    {
                        ctx.AuctionInfos.Add(info);
                    }
                    else
                    {
                        ctx.Entry(dbInfo).CurrentValues.SetValues(info);
                    }
                    ctx.SaveChanges();
                    this.AuctionID = info.AuctionID;
                }
                return;
            }
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
