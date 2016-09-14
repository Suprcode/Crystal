namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingAttrs2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovementInfoes", "SourceMapIndex", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovementInfoes", "SourceMapIndex");
        }
    }
}
