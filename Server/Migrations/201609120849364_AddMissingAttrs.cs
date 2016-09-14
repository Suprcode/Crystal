namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingAttrs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountInfoes", "DBGold", c => c.Long(nullable: false));
            AddColumn("dbo.AccountInfoes", "DBCredit", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountInfoes", "DBCredit");
            DropColumn("dbo.AccountInfoes", "DBGold");
        }
    }
}
