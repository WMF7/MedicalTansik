namespace MedicalTansik.Migrations
{
	using MedicalTansik.Models;
	using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MedicalTansik.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MedicalTansik.Models.ApplicationDbContext context)
        {
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Id = 1, Name = "جراحة الانف والاذن والحنجرة" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Id = 2, Name = "الجراحة العامة" }); 
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Id = 3, Name = "التوليد وامراض النسا" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Id = 4, Name = "طب الاطفال" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Id = 5, Name = "الباطنة العامة" });
            context.SaveChanges();

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }

    
    }
}
