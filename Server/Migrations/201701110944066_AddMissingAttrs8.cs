namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingAttrs8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConquestFlagInfoes",
                c => new
                    {
                        Index = c.Int(nullable: false, identity: true),
                        DBLocation = c.String(),
                        Name = c.String(),
                        FileName = c.String(),
                        ConquestInfoIndex = c.Int(nullable: false),
                        ConquestFlagType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Index);
            
            AddColumn("dbo.ConquestInfoes", "DBKingLocation", c => c.String());
            AddColumn("dbo.ConquestInfoes", "DBKingSize", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConquestInfoes", "DBKingSize");
            DropColumn("dbo.ConquestInfoes", "DBKingLocation");
            DropTable("dbo.ConquestFlagInfoes");
        }
    }
}
