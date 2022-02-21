namespace MedicalTansik.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_is_academic : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Desires", "MedicalSubject_Id", "dbo.MedicalSubjects");
            DropIndex("dbo.Desires", new[] { "MedicalSubject_Id" });
            AddColumn("dbo.Desires", "IsAcademic", c => c.Boolean(nullable: false, defaultValue: false));
            AlterColumn("dbo.Desires", "MedicalSubject_Id", c => c.Long());
            CreateIndex("dbo.Desires", "MedicalSubject_Id");
            AddForeignKey("dbo.Desires", "MedicalSubject_Id", "dbo.MedicalSubjects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Desires", "MedicalSubject_Id", "dbo.MedicalSubjects");
            DropIndex("dbo.Desires", new[] { "MedicalSubject_Id" });
            AlterColumn("dbo.Desires", "MedicalSubject_Id", c => c.Long(nullable: false));
            DropColumn("dbo.Desires", "IsAcademic");
            CreateIndex("dbo.Desires", "MedicalSubject_Id");
            AddForeignKey("dbo.Desires", "MedicalSubject_Id", "dbo.MedicalSubjects", "Id", cascadeDelete: true);
        }
    }
}
