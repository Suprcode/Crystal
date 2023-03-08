using Server.MirEnvir;

namespace Server.MirDatabase
{
    public class AuctionInfo
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        public ulong AuctionID; 

        public UserItem Item;
        public DateTime ConsignmentDate;
        public uint Price, CurrentBid;

        public int SellerIndex, CurrentBuyerIndex;
        public CharacterInfo SellerInfo, CurrentBuyerInfo;

        public bool Expired, Sold;

        public MarketItemType ItemType;

        public AuctionInfo()
        {
            
        }


        public AuctionInfo(CharacterInfo info, UserItem item, uint price, MarketItemType itemType)
        {
            AuctionID = ++Envir.NextAuctionID;
            SellerIndex = info.Index;
            SellerInfo = info;
            ConsignmentDate = Envir.Now;
            Item = item;
            Price = price;
            ItemType = itemType;

            if (itemType == MarketItemType.Auction)
            {
                CurrentBid = Price;
            }
        }

        public AuctionInfo(BinaryReader reader, int version, int customversion)
        {
            AuctionID = reader.ReadUInt64();

            Item = new UserItem(reader, version, customversion);
            ConsignmentDate = DateTime.FromBinary(reader.ReadInt64());
            Price = reader.ReadUInt32();
            SellerIndex = reader.ReadInt32();
            Expired = reader.ReadBoolean();
            Sold = reader.ReadBoolean();

            if (version > 79)
            {
                ItemType = (MarketItemType)reader.ReadByte();

                CurrentBid = reader.ReadUInt32();

                if (CurrentBid < Price)
                    CurrentBid = Price;

                CurrentBuyerIndex = reader.ReadInt32();
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(AuctionID);

            Item.Save(writer);
            writer.Write(ConsignmentDate.ToBinary());
            writer.Write(Price);

            writer.Write(SellerIndex);

            writer.Write(Expired);
            writer.Write(Sold);

            writer.Write((byte)ItemType);
            writer.Write(CurrentBid);
            writer.Write(CurrentBuyerIndex);
        }

        private string GetSellerLabel(bool userMatch)
        {
            switch (ItemType)
            {
                case MarketItemType.GameShop:
                    return "";
                case MarketItemType.Consign:
                    return userMatch ? (Sold ? "Sold" : (Expired ? "Expired" : "For Sale")) : SellerInfo.Name;
                case MarketItemType.Auction:
                    return userMatch ? (Sold ? "Sold" : (Expired ? "Expired" : CurrentBid > Price ? "Bid Met" : "No Bid")) : SellerInfo.Name;
            }

            return "";
        }

        public ClientAuction CreateClientAuction(bool userMatch)
        {
            return new ClientAuction
            {
                AuctionID = AuctionID,
                Item = Item,
                Seller = GetSellerLabel(userMatch),
                Price = ItemType == MarketItemType.Auction ? CurrentBid : Price,
                ConsignmentDate = ConsignmentDate,
                ItemType = ItemType
            };
        }
    }
}
