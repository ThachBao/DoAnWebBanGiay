namespace DoAnWebBanGiay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserProF : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProFs",
                c => new
                    {
                        UserProfileId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FullName = c.String(nullable: false, maxLength: 100),
                        PhoneNumber = c.String(nullable: false, maxLength: 15),
                        Address = c.String(nullable: false, maxLength: 250),
                        BirthDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserProfileId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProFs", "UserId", "dbo.Users");
            DropIndex("dbo.UserProFs", new[] { "UserId" });
            DropTable("dbo.UserProFs");
        }
    }
}
