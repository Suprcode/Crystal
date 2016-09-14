namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveQuestAttrsFromDB : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.QuestInfoes", "DBNpcIndex");
            DropColumn("dbo.QuestInfoes", "DBFinishNpcIndex");
            DropColumn("dbo.QuestInfoes", "DBDescription");
            DropColumn("dbo.QuestInfoes", "DBTaskDescription");
            DropColumn("dbo.QuestInfoes", "DBCompletionDescription");
            DropColumn("dbo.QuestInfoes", "DBGoldReward");
            DropColumn("dbo.QuestInfoes", "DBExpReward");
            DropColumn("dbo.QuestInfoes", "DBCreditReward");
            DropColumn("dbo.QuestInfoes", "DBFixedRewards");
            DropColumn("dbo.QuestInfoes", "DBSelectRewards");
            DropColumn("dbo.QuestProgressInfoes", "DbTaskList");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestProgressInfoes", "DbTaskList", c => c.String());
            AddColumn("dbo.QuestInfoes", "DBSelectRewards", c => c.String());
            AddColumn("dbo.QuestInfoes", "DBFixedRewards", c => c.String());
            AddColumn("dbo.QuestInfoes", "DBCreditReward", c => c.Long(nullable: false));
            AddColumn("dbo.QuestInfoes", "DBExpReward", c => c.Long(nullable: false));
            AddColumn("dbo.QuestInfoes", "DBGoldReward", c => c.Long(nullable: false));
            AddColumn("dbo.QuestInfoes", "DBCompletionDescription", c => c.String());
            AddColumn("dbo.QuestInfoes", "DBTaskDescription", c => c.String());
            AddColumn("dbo.QuestInfoes", "DBDescription", c => c.String());
            AddColumn("dbo.QuestInfoes", "DBFinishNpcIndex", c => c.Long(nullable: false));
            AddColumn("dbo.QuestInfoes", "DBNpcIndex", c => c.Long(nullable: false));
        }
    }
}
