namespace MedicalTansik.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_national_id_to_users : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NatId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NatId");
        }
    }
}
