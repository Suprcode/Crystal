using System;
using System.Data.Entity;
using System.Diagnostics;
using Server.MirEnvir;
using Server.MirObjects;

namespace Server.MirDatabase
{
    public class DataContext : DbContext
    {
        public DbSet<CharacterInfo> CharacterInfos { get; set; }
        public DbSet<AccountInfo> AccountInfos { get; set; }
        public DbSet<InventoryItem> Inventories { get; set; }
        public DbSet<EquipmentItem> Equipments { get; set; }
        public DbSet<QuestInventoryItem> QuestInventories { get; set; }
        public DbSet<UserMagic> UserMagics { get; set; }
        public DbSet<CurrentRefineItem> CurrentRefines { get; set; }
        //public DbSet<PetInfo> PetInfos { get; set; } 
        public DbSet<UserBuff> UserBuffs { get; set; }
        public DbSet<MailInfo> Mails { get; set; }
        public DbSet<MailItem> MailItems { get; set; }
        public DbSet<UserIntelligentCreature> UserIntelligentCreatures { get; set; }
        public DbSet<FriendInfo> Friends { get; set; }
        public DbSet<GameShopPurchase> GameShopPurchases { get; set; }
        public DbSet<QuestProgressInfo> QuestProgressInfos { get; set; }

        public DbSet<UserItem> UserItems { get; set; }

        public DbSet<ItemInfo> ItemInfos { get; set; }
        public DbSet<MagicInfo> MagicInfos { get; set; }
        public DbSet<MonsterInfo> MonsterInfos { get; set; }
        public DbSet<MapInfo> MapInfos { get; set; }
        public DbSet<SafeZoneInfo> SafeZoneInfos { get; set; }
        public DbSet<MovementInfo> MovementInfos { get; set; }
        public DbSet<RespawnInfo> RespawnInfos { get; set; }
        public DbSet<NPCInfo> NpcInfos { get; set; }
        public DbSet<MineZone> MineZones { get; set; }
        public DbSet<QuestInfo> QuestInfos { get; set; }
        public DbSet<DragonInfo> DragonInfos { get; set; }
        public DbSet<GameShopItem> GameShopItems { get; set; }
        public DbSet<ConquestInfo> ConquestInfos { get; set; }
        public DbSet<ConquestArcherInfo> ConquestArcherInfos { get; set; }
        public DbSet<ConquestGateInfo> ConquestGateInfos { get; set; }
        public DbSet<ConquestWallInfo> ConquestWallInfos { get; set; }
        public DbSet<ConquestSiegeInfo> ConquestSiegeInfos { get; set; }

        public DbSet<AuctionInfo> AuctionInfos { get; set; }
        public DbSet<RespawnSave> RespawnSaves { get; set; }
        public DbSet<GuildObject> Guilds { get; set; }

        public DataContext() : base("Mir")
        {
#if DEBUG
            Database.Log = s => Debug.WriteLine(s);
#endif
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
