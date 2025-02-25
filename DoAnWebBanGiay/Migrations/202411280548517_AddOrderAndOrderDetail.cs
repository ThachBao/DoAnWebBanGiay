namespace DoAnWebBanGiay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddOrderAndOrderDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderDetails",
                c => new
                {
                    OrderDetailId = c.Int(nullable: false, identity: true),
                    OrderId = c.Int(nullable: false),
                    ProId = c.Int(nullable: false),
                    ProductSizeId = c.Int(nullable: false),
                    Quantity = c.Int(nullable: false),
                    Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.OrderDetailId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true) // Cascade delete chỉ giữ tại Orders
                .ForeignKey("dbo.Products", t => t.ProId, cascadeDelete: false) // Không cascade delete tại Products
                .ForeignKey("dbo.ProductSizes", t => t.ProductSizeId, cascadeDelete: false) // Không cascade delete tại ProductSizes
                .Index(t => t.OrderId)
                .Index(t => t.ProId)
                .Index(t => t.ProductSizeId);

            CreateTable(
                "dbo.Orders",
                c => new
                {
                    OrderId = c.Int(nullable: false, identity: true),
                    UserId = c.Int(nullable: false),
                    ShippingAddress = c.String(nullable: false, maxLength: 250),
                    PaymentMethod = c.String(nullable: false, maxLength: 50),
                    OrderDate = c.DateTime(nullable: false),
                    TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true) // Cascade delete tại Users
                .Index(t => t.UserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "ProductSizeId", "dbo.ProductSizes");
            DropForeignKey("dbo.OrderDetails", "ProId", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductSizeId" });
            DropIndex("dbo.OrderDetails", new[] { "ProId" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
        }
    }
}
