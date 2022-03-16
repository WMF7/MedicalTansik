using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MedicalTansik.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Student> students { set; get; }
        public DbSet<Desire> Desires { set; get; }
        public DbSet<Announcment> Announcments { set; get; }
        public DbSet<GradeYear> GradeYears { set; get; }
        public DbSet<StudentDesire> StudentDesires { set; get; }
        public DbSet<MedicalSubject> MedicalSubjects { set; get; }
        public DbSet<StudentSubjectDegree> StudentSubjectDegrees { set; get; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            base.OnModelCreating(modelBuilder);
            
        }

		public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

		
    }
}

