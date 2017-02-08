namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecreateQuestProgressTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestProgressInfoes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Index = c.Int(nullable: false),
                        StartDateTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        EndDateTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        CharacterIndex = c.Int(nullable: false),
                        DbKillTaskCount = c.String(),
                        DbItemTaskCount = c.String(),
                        DbFlagTaskSet = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.CharacterInfoes", t => t.CharacterIndex, cascadeDelete: true)
                .Index(t => t.CharacterIndex);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestProgressInfoes", "CharacterIndex", "dbo.CharacterInfoes");
            DropIndex("dbo.QuestProgressInfoes", new[] { "CharacterIndex" });
            DropTable("dbo.QuestProgressInfoes");
        }
    }
}
