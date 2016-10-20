namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DestoryGameShopItemTable : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.GameShopItems");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GameShopItems",
                c => new
                    {
                        ItemIndex = c.Int(nullable: false, identity: true),
                        GIndex = c.Int(nullable: false),
                        ItemInfoIndex = c.Int(nullable: false),
                        Class = c.String(),
                        Category = c.String(),
                        Stock = c.Int(nullable: false),
                        iStock = c.Boolean(nullable: false),
                        Deal = c.Boolean(nullable: false),
                        TopItem = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ItemIndex);
            
        }
    }
}
