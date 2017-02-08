namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingAttrs9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserItems", "DBSlots", c => c.String());
            AddColumn("dbo.UserBuffs", "CasterName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserBuffs", "CasterName");
            DropColumn("dbo.UserItems", "DBSlots");
        }
    }
}
