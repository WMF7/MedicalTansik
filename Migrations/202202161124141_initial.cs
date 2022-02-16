namespace MedicalTansik.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Announcments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GradeYear_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GradeYears", t => t.GradeYear_Id, cascadeDelete: true)
                .Index(t => t.GradeYear_Id);
            
            CreateTable(
                "dbo.GradeYears",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        year = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Desires",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Positions = c.Int(nullable: false),
                        active = c.Boolean(nullable: false),
                        MedicalSubject_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MedicalSubjects", t => t.MedicalSubject_Id, cascadeDelete: true)
                .Index(t => t.MedicalSubject_Id);
            
            CreateTable(
                "dbo.MedicalSubjects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.StudentDesires",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        rank = c.Int(nullable: false),
                        Announcment_Id = c.Long(),
                        Desire_Id = c.Long(nullable: false),
                        Student_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Announcments", t => t.Announcment_Id)
                .ForeignKey("dbo.Desires", t => t.Desire_Id, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_Id, cascadeDelete: true)
                .Index(t => t.Announcment_Id)
                .Index(t => t.Desire_Id)
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NatId = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Gender = c.String(nullable: false),
                        Nationality = c.String(nullable: false),
                        City = c.String(),
                        Governate = c.String(),
                        BirthDay = c.String(nullable: false),
                        BirthMonth = c.String(nullable: false),
                        BirthYear = c.String(nullable: false),
                        Address = c.String(),
                        Total = c.String(nullable: false),
                        Percentage = c.String(nullable: false),
                        GradeYear_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GradeYears", t => t.GradeYear_Id, cascadeDelete: true)
                .Index(t => t.GradeYear_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsStudent = c.Boolean(nullable: false),
                        DataConfirmed = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Student_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.Student_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Student_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentDesires", "Student_Id", "dbo.Students");
            DropForeignKey("dbo.Students", "GradeYear_Id", "dbo.GradeYears");
            DropForeignKey("dbo.StudentDesires", "Desire_Id", "dbo.Desires");
            DropForeignKey("dbo.StudentDesires", "Announcment_Id", "dbo.Announcments");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Desires", "MedicalSubject_Id", "dbo.MedicalSubjects");
            DropForeignKey("dbo.Announcments", "GradeYear_Id", "dbo.GradeYears");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Student_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Students", new[] { "GradeYear_Id" });
            DropIndex("dbo.StudentDesires", new[] { "Student_Id" });
            DropIndex("dbo.StudentDesires", new[] { "Desire_Id" });
            DropIndex("dbo.StudentDesires", new[] { "Announcment_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Desires", new[] { "MedicalSubject_Id" });
            DropIndex("dbo.Announcments", new[] { "GradeYear_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Students");
            DropTable("dbo.StudentDesires");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.MedicalSubjects");
            DropTable("dbo.Desires");
            DropTable("dbo.GradeYears");
            DropTable("dbo.Announcments");
        }
    }
}
