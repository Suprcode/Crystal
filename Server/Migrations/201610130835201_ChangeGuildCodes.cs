namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGuildCodes : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GuildObjects", newName: "BaseGuildObjects");
            AddColumn("dbo.BaseGuildObjects", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseGuildObjects", "Discriminator");
            RenameTable(name: "dbo.BaseGuildObjects", newName: "GuildObjects");
        }
    }
}
