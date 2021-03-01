namespace LookaukwatApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductModels", "Provider_Id", c => c.String());
            AddColumn("dbo.ProductModels", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductModels", "Stock", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductModels", "Stock");
            DropColumn("dbo.ProductModels", "IsActive");
            DropColumn("dbo.ProductModels", "Provider_Id");
        }
    }
}
