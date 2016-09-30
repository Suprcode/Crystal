namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingAttrs4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameShopItems", "DBGoldPrice", c => c.Long(nullable: false));
            AddColumn("dbo.GameShopItems", "DBCreditPrice", c => c.Long(nullable: false));
            AddColumn("dbo.GameShopItems", "DBCount", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameShopItems", "DBCount");
            DropColumn("dbo.GameShopItems", "DBCreditPrice");
            DropColumn("dbo.GameShopItems", "DBGoldPrice");
        }
    }
}
