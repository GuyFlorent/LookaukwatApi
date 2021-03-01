namespace LookaukwatApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsLOOkaukwat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductModels", "IsLookaukwat", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductModels", "IsParticulier", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductModels", "IsParticulier");
            DropColumn("dbo.ProductModels", "IsLookaukwat");
        }
    }
}
