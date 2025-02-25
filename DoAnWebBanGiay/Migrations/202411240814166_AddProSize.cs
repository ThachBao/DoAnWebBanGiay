namespace DoAnWebBanGiay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProSize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductSizes",
                c => new
                    {
                        ProductSizeId = c.Int(nullable: false, identity: true),
                        ProId = c.Int(nullable: false),
                        Size = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductSizeId)
                .ForeignKey("dbo.Products", t => t.ProId, cascadeDelete: true)
                .Index(t => t.ProId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductSizes", "ProId", "dbo.Products");
            DropIndex("dbo.ProductSizes", new[] { "ProId" });
            DropTable("dbo.ProductSizes");
        }
    }
}
