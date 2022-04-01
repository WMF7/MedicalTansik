namespace MedicalTansik.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_student_rank : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Rank", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "Rank");
        }
    }
}
