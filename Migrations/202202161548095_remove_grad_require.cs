namespace MedicalTansik.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remove_grad_require : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Students", "GradeYear_Id", "dbo.GradeYears");
            DropIndex("dbo.Students", new[] { "GradeYear_Id" });
            AlterColumn("dbo.Students", "GradeYear_Id", c => c.Long());
            CreateIndex("dbo.Students", "GradeYear_Id");
            AddForeignKey("dbo.Students", "GradeYear_Id", "dbo.GradeYears", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "GradeYear_Id", "dbo.GradeYears");
            DropIndex("dbo.Students", new[] { "GradeYear_Id" });
            AlterColumn("dbo.Students", "GradeYear_Id", c => c.Long(nullable: false));
            CreateIndex("dbo.Students", "GradeYear_Id");
            AddForeignKey("dbo.Students", "GradeYear_Id", "dbo.GradeYears", "Id", cascadeDelete: true);
        }
    }
}
