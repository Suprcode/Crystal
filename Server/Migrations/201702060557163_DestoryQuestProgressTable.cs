namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DestoryQuestProgressTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestProgressInfoes", "CharacterIndex", "dbo.CharacterInfoes");
            DropIndex("dbo.QuestProgressInfoes", new[] { "CharacterIndex" });
            DropTable("dbo.QuestProgressInfoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.QuestProgressInfoes",
                c => new
                    {
                        Index = c.Int(nullable: false, identity: true),
                        StartDateTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        EndDateTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        CharacterIndex = c.Int(nullable: false),
                        DbKillTaskCount = c.String(),
                        DbItemTaskCount = c.String(),
                        DbFlagTaskSet = c.String(),
                    })
                .PrimaryKey(t => t.Index);
            
            CreateIndex("dbo.QuestProgressInfoes", "CharacterIndex");
            AddForeignKey("dbo.QuestProgressInfoes", "CharacterIndex", "dbo.CharacterInfoes", "Index", cascadeDelete: true);
        }
    }
}
