namespace LookaukwatApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DATENull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CommandModels", "CommandDate", c => c.DateTime());
            AlterColumn("dbo.CommandModels", "DeliveredDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CommandModels", "DeliveredDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CommandModels", "CommandDate", c => c.DateTime(nullable: false));
        }
    }
}
