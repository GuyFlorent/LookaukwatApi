namespace LookaukwatApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Town = c.String(),
                        Street = c.String(),
                        Price = c.Int(nullable: false),
                        DateAdd = c.DateTime(nullable: false),
                        SearchOrAskJob = c.String(),
                        ViewNumber = c.Int(nullable: false),
                        ApartSurface = c.Int(),
                        RoomNumber = c.Int(),
                        FurnitureOrNot = c.String(),
                        Type = c.String(),
                        RubriqueHouse = c.String(),
                        TypeHouse = c.String(),
                        FabricMaterialeHouse = c.String(),
                        ColorHouse = c.String(),
                        StateHouse = c.String(),
                        TypeContract = c.String(),
                        ActivitySector = c.String(),
                        RubriqueMode = c.String(),
                        TypeMode = c.String(),
                        BrandMode = c.String(),
                        UniversMode = c.String(),
                        SizeMode = c.String(),
                        ColorMode = c.String(),
                        StateMode = c.String(),
                        Type1 = c.String(),
                        Brand = c.String(),
                        Model = c.String(),
                        Capacity = c.String(),
                        RubriqueVehicule = c.String(),
                        BrandVehicule = c.String(),
                        ModelVehicule = c.String(),
                        TypeVehicule = c.String(),
                        PetrolVehicule = c.String(),
                        FirstYearVehicule = c.String(),
                        YearVehicule = c.String(),
                        MileageVehicule = c.String(),
                        NumberOfDoorVehicule = c.String(),
                        ColorVehicule = c.String(),
                        StateVehicule = c.String(),
                        GearBoxVehicule = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Category_id = c.Int(),
                        Coordinate_id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.CategoryModels", t => t.Category_id)
                .ForeignKey("dbo.ProductCoordinateModels", t => t.Coordinate_id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Category_id)
                .Index(t => t.Coordinate_id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.CategoryModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ProductCoordinateModels",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Lat = c.String(),
                        Lon = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.ImageProcductModels",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        Image = c.String(),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.ProductModels", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ParrainModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date_createParrain = c.DateTime(nullable: false),
                        ParrainEmail = c.String(),
                        ParrainFirstName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductModels", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ImageProcductModels", "ProductId", "dbo.ProductModels");
            DropForeignKey("dbo.ProductModels", "Coordinate_id", "dbo.ProductCoordinateModels");
            DropForeignKey("dbo.ProductModels", "Category_id", "dbo.CategoryModels");
            DropIndex("dbo.ImageProcductModels", new[] { "ProductId" });
            DropIndex("dbo.ProductModels", new[] { "User_Id" });
            DropIndex("dbo.ProductModels", new[] { "Coordinate_id" });
            DropIndex("dbo.ProductModels", new[] { "Category_id" });
            DropTable("dbo.ParrainModels");
            DropTable("dbo.ImageProcductModels");
            DropTable("dbo.ProductCoordinateModels");
            DropTable("dbo.CategoryModels");
            DropTable("dbo.ProductModels");
        }
    }
}
