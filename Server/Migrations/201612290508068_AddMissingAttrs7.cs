namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingAttrs7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountInfoes", "AdminAccount", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountInfoes", "AdminAccount");
        }
    }
}
