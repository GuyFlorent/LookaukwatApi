namespace LookaukwatApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isdeletebyuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommandModels", "IsDeleteByUser", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommandModels", "IsDeleteByUser");
        }
    }
}
