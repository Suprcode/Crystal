using Server.MirDatabase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MirDatabase
{
    class MirDbContext : DbContext
    {
        public DbSet<AccountInfo> Accounts { get; set; }
        public DbSet<CharacterInfo> Characters { get; set; }
        public DbSet<ConquestInfo> Conquests { get; set; }
        public DbSet<DragonInfo> Dragons { get; set; }
        public DbSet<MagicInfo> Magics { get; set; }
        public DbSet<MailInfo> Mails { get; set; }
        public DbSet<MapInfo> Maps { get; set; }
        public DbSet<MonsterInfo> Monsters { get; set; }
        public DbSet<MovementInfo> Movements { get; set; }
        public DbSet<NPCInfo> NPCs { get; set; }
        public DbSet<QuestInfo> Quests { get; set; }
        public DbSet<QuestProgressInfo> QuestProgresses { get; set; }
        public DbSet<RespawnInfo> Respawns { get; set; }
        public DbSet<SafeZoneInfo> SafeZones { get; set; }

        public MirDbContext() :base("MirDatabase")
        { }

    }
}
