namespace LookaukwatApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OtherNumberCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductModels", "CallNumber", c => c.Int(nullable: false));
            AddColumn("dbo.ProductModels", "MessageNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductModels", "MessageNumber");
            DropColumn("dbo.ProductModels", "CallNumber");
        }
    }
}
