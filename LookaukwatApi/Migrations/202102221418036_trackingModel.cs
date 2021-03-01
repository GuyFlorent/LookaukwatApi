namespace LookaukwatApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trackingModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrackingCommandModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Town = c.String(),
                        Street = c.String(),
                        Road = c.String(),
                        Date = c.DateTime(nullable: false),
                        Lat = c.String(),
                        Lon = c.String(),
                        Command_Id = c.Int(),
                        UserAgent_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommandModels", t => t.Command_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserAgent_Id)
                .Index(t => t.Command_Id)
                .Index(t => t.UserAgent_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrackingCommandModels", "UserAgent_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TrackingCommandModels", "Command_Id", "dbo.CommandModels");
            DropIndex("dbo.TrackingCommandModels", new[] { "UserAgent_Id" });
            DropIndex("dbo.TrackingCommandModels", new[] { "Command_Id" });
            DropTable("dbo.TrackingCommandModels");
        }
    }
}
