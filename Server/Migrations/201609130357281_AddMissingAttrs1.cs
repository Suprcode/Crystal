namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingAttrs1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CharacterInfoes", "DBCurrentLocation", c => c.String());
            AddColumn("dbo.CharacterInfoes", "DBBindLocatoin", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CharacterInfoes", "DBBindLocatoin");
            DropColumn("dbo.CharacterInfoes", "DBCurrentLocation");
        }
    }
}
