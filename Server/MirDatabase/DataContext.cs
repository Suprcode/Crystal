using System;
using System.Data.Entity;

namespace Server.MirDatabase
{
    class DataContext : DbContext
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

        public DataContext() : base("Mir")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
