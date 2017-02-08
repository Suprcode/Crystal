namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DestoryGuildBuffTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GuildBuffs", "GuildIndex", "dbo.BaseGuildObjects");
            DropIndex("dbo.GuildBuffs", new[] { "GuildIndex" });
            DropTable("dbo.GuildBuffs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GuildBuffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Active = c.Boolean(nullable: false),
                        ActiveTimeRemaining = c.Int(nullable: false),
                        GuildIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.GuildBuffs", "GuildIndex");
            AddForeignKey("dbo.GuildBuffs", "GuildIndex", "dbo.BaseGuildObjects", "Guildindex", cascadeDelete: true);
        }
    }
}
