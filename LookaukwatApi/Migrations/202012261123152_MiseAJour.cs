namespace LookaukwatApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MiseAJour : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImageProcductModels", "ImageMobile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImageProcductModels", "ImageMobile");
        }
    }
}
