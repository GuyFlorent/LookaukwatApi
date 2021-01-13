namespace LookaukwatApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeProdut : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ImageProcductModels", "ProductId", "dbo.ProductModels");
            DropIndex("dbo.ImageProcductModels", new[] { "ProductId" });
            AddColumn("dbo.ImageProcductModels", "ProductModel_id", c => c.Int());
            CreateIndex("dbo.ImageProcductModels", "ProductModel_id");
            AddForeignKey("dbo.ImageProcductModels", "ProductModel_id", "dbo.ProductModels", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImageProcductModels", "ProductModel_id", "dbo.ProductModels");
            DropIndex("dbo.ImageProcductModels", new[] { "ProductModel_id" });
            DropColumn("dbo.ImageProcductModels", "ProductModel_id");
            CreateIndex("dbo.ImageProcductModels", "ProductId");
            AddForeignKey("dbo.ImageProcductModels", "ProductId", "dbo.ProductModels", "id", cascadeDelete: true);
        }
    }
}
