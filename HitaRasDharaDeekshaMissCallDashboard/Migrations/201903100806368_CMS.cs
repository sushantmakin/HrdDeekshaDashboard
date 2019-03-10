namespace HitaRasDharaDeekshaMissCallDashboard.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CMS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CMSViewModels",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        Value = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CMSViewModels");
        }
    }
}
