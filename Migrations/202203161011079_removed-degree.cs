namespace MedicalTansik.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeddegree : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentSubjectDegrees",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Takdeer = c.String(),
                        MedicalSubject_Id = c.Long(),
                        Student_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MedicalSubjects", t => t.MedicalSubject_Id)
                .ForeignKey("dbo.Students", t => t.Student_Id)
                .Index(t => t.MedicalSubject_Id)
                .Index(t => t.Student_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentSubjectDegrees", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.StudentSubjectDegrees", "MedicalSubject_Id", "dbo.MedicalSubjects");
            DropIndex("dbo.StudentSubjectDegrees", new[] { "Student_Id" });
            DropIndex("dbo.StudentSubjectDegrees", new[] { "MedicalSubject_Id" });
            DropTable("dbo.StudentSubjectDegrees");
        }
    }
}
