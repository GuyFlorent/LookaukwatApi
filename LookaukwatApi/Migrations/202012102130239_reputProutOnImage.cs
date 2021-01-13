namespace LookaukwatApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reputProutOnImage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ImageProcductModels", "ProductModel_id", "dbo.ProductModels");
            DropIndex("dbo.ImageProcductModels", new[] { "ProductModel_id" });
            DropColumn("dbo.ImageProcductModels", "ProductId");
            RenameColumn(table: "dbo.ImageProcductModels", name: "ProductModel_id", newName: "ProductId");
            AlterColumn("dbo.ImageProcductModels", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.ImageProcductModels", "ProductId");
            AddForeignKey("dbo.ImageProcductModels", "ProductId", "dbo.ProductModels", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImageProcductModels", "ProductId", "dbo.ProductModels");
            DropIndex("dbo.ImageProcductModels", new[] { "ProductId" });
            AlterColumn("dbo.ImageProcductModels", "ProductId", c => c.Int());
            RenameColumn(table: "dbo.ImageProcductModels", name: "ProductId", newName: "ProductModel_id");
            AddColumn("dbo.ImageProcductModels", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.ImageProcductModels", "ProductModel_id");
            AddForeignKey("dbo.ImageProcductModels", "ProductModel_id", "dbo.ProductModels", "id");
        }
    }
}
