namespace LookaukwatApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeStock2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductModels", "Stock", c => c.Int(nullable: false));
            DropColumn("dbo.ProductModels", "Stock1");
            DropColumn("dbo.ProductModels", "Stock2");
            DropColumn("dbo.ProductModels", "Stock3");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductModels", "Stock3", c => c.Int());
            AddColumn("dbo.ProductModels", "Stock2", c => c.Int());
            AddColumn("dbo.ProductModels", "Stock1", c => c.Int());
            AlterColumn("dbo.ProductModels", "Stock", c => c.Int());
        }
    }
}
