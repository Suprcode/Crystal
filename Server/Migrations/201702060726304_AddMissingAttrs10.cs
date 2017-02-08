namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingAttrs10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserItems", "DBAwake", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserItems", "DBAwake");
        }
    }
}
