using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Server.MirDatabase
{
    public class ServerDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Settings.ServerDbConnectionString);
        }
        public DbSet<ItemInfo> Items { get; set; }
        public DbSet<MagicInfo> Magics { get; set; }

        public DbSet<MapInfo> Maps { get; set; }
        public DbSet<RespawnInfo> Respawns { get; set; }
        public DbSet<MovementInfo> Movements { get; set;  }

        public DbSet<MonsterInfo> Monsters { get; set; }
        public DbSet<NPCInfo> NpcInfos { get; set; }
        public DbSet<QuestInfo> Quests { get; set; }

        public DbSet<GameShopItem> GameShopItems { get; set; }
        public DbSet<ConquestInfo> ConquestInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemInfo>().HasKey(i => i.Index);
            modelBuilder.Entity<ItemInfo>().Ignore(i => i.RandomStats);
            modelBuilder.Entity<GameShopItem>().HasKey(i => i.GIndex);
            modelBuilder.Entity<GameShopItem>().Ignore(i => i.Info);

            base.OnModelCreating(modelBuilder);
        }
    }

    public class AccountDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Settings.AccountDbConnectionString);
        }
    }
}
