namespace DoAnWebBanGiay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCart : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Carts", "ProductSizeId");
            AddForeignKey("dbo.Carts", "ProductSizeId", "dbo.ProductSizes", "ProductSizeId", cascadeDelete: false); // Không sử dụng cascade
        }

        public override void Down()
        {
            DropForeignKey("dbo.Carts", "ProductSizeId", "dbo.ProductSizes");
            DropIndex("dbo.Carts", new[] { "ProductSizeId" });
        }
    }
}
