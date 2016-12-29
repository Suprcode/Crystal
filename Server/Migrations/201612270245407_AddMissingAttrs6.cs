namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingAttrs6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserItems", "DBCount", c => c.Long(nullable: false));
            AddColumn("dbo.UserItems", "DBGemCount", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserItems", "DBGemCount");
            DropColumn("dbo.UserItems", "DBCount");
        }
    }
}
