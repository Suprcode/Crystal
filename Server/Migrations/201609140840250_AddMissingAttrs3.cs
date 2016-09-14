namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingAttrs3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NPCInfoes", "IsRobot", c => c.Boolean(nullable: false));
            AlterColumn("dbo.NPCInfoes", "Image", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NPCInfoes", "Image", c => c.Byte(nullable: false));
            DropColumn("dbo.NPCInfoes", "IsRobot");
        }
    }
}
