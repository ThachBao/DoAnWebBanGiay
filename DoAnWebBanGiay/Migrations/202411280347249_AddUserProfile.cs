namespace DoAnWebBanGiay.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserProfile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        PhoneNumber = c.String(nullable: false, maxLength: 15),
                        Address = c.String(nullable: false, maxLength: 250),
                        BirthDate = c.DateTime(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfiles", "User_UserId", "dbo.Users");
            DropIndex("dbo.UserProfiles", new[] { "User_UserId" });
            DropTable("dbo.UserProfiles");
        }
    }
}
