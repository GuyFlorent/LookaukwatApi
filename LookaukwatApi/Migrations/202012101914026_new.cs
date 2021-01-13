namespace LookaukwatApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductModels", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.ProductModels", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.ProductModels", "Street", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductModels", "Street", c => c.String());
            AlterColumn("dbo.ProductModels", "Description", c => c.String());
            AlterColumn("dbo.ProductModels", "Title", c => c.String());
        }
    }
}
