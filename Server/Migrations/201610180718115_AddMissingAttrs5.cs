namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingAttrs5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuctionInfoes", "ConsignmentDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.AuctionInfoes", "UserItemUniqueID");
            AddForeignKey("dbo.AuctionInfoes", "UserItemUniqueID", "dbo.UserItems", "UniqueID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuctionInfoes", "UserItemUniqueID", "dbo.UserItems");
            DropIndex("dbo.AuctionInfoes", new[] { "UserItemUniqueID" });
            DropColumn("dbo.AuctionInfoes", "ConsignmentDate");
        }
    }
}
