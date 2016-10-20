namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStorage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StorageItems",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        UserItemUniqueID = c.Long(),
                        AccountIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AccountInfoes", t => t.AccountIndex, cascadeDelete: true)
                .ForeignKey("dbo.UserItems", t => t.UserItemUniqueID)
                .Index(t => t.UserItemUniqueID)
                .Index(t => t.AccountIndex);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StorageItems", "UserItemUniqueID", "dbo.UserItems");
            DropForeignKey("dbo.StorageItems", "AccountIndex", "dbo.AccountInfoes");
            DropIndex("dbo.StorageItems", new[] { "AccountIndex" });
            DropIndex("dbo.StorageItems", new[] { "UserItemUniqueID" });
            DropTable("dbo.StorageItems");
        }
    }
}
