namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGuildTables : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GuildObjects", t => t.GuildIndex, cascadeDelete: true)
                .Index(t => t.GuildIndex);
            
            CreateTable(
                "dbo.GuildMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        LastLogin = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        hasvoted = c.Boolean(nullable: false),
                        Online = c.Boolean(nullable: false),
                        RankId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ranks", t => t.RankId, cascadeDelete: true)
                .Index(t => t.RankId);
            
            CreateTable(
                "dbo.Ranks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Index = c.Int(nullable: false),
                        Options = c.Byte(nullable: false),
                        GuildIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.GuildObjects", t => t.GuildIndex, cascadeDelete: true)
                .Index(t => t.GuildIndex);
            
            CreateTable(
                "dbo.GuildStorageItems",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ItemUniqueID = c.Long(),
                        UserId = c.Long(),
                        GuildIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.GuildObjects", t => t.GuildIndex, cascadeDelete: true)
                .ForeignKey("dbo.UserItems", t => t.ItemUniqueID)
                .Index(t => t.ItemUniqueID)
                .Index(t => t.GuildIndex);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GuildStorageItems", "ItemUniqueID", "dbo.UserItems");
            DropForeignKey("dbo.GuildStorageItems", "GuildIndex", "dbo.GuildObjects");
            DropForeignKey("dbo.GuildMembers", "RankId", "dbo.Ranks");
            DropForeignKey("dbo.Ranks", "GuildIndex", "dbo.GuildObjects");
            DropForeignKey("dbo.GuildBuffs", "GuildIndex", "dbo.GuildObjects");
            DropIndex("dbo.GuildStorageItems", new[] { "GuildIndex" });
            DropIndex("dbo.GuildStorageItems", new[] { "ItemUniqueID" });
            DropIndex("dbo.Ranks", new[] { "GuildIndex" });
            DropIndex("dbo.GuildMembers", new[] { "RankId" });
            DropIndex("dbo.GuildBuffs", new[] { "GuildIndex" });
            DropTable("dbo.GuildStorageItems");
            DropTable("dbo.Ranks");
            DropTable("dbo.GuildMembers");
            DropTable("dbo.GuildBuffs");
        }
    }
}
