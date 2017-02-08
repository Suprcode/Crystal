namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecreateGuildBuffTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GuildBuffs",
                c => new
                    {
                        Index = c.Int(nullable: false, identity: true),
                        Id = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        ActiveTimeRemaining = c.Int(nullable: false),
                        GuildIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Index)
                .ForeignKey("dbo.BaseGuildObjects", t => t.GuildIndex, cascadeDelete: true)
                .Index(t => t.GuildIndex);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GuildBuffs", "GuildIndex", "dbo.BaseGuildObjects");
            DropIndex("dbo.GuildBuffs", new[] { "GuildIndex" });
            DropTable("dbo.GuildBuffs");
        }
    }
}
