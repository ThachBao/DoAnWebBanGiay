namespace DoAnWebBanGiay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIdToCart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Carts", "UserId");
            AddForeignKey("dbo.Carts", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "UserId", "dbo.Users");
            DropIndex("dbo.Carts", new[] { "UserId" });
            DropColumn("dbo.Carts", "UserId");
        }
    }
}
