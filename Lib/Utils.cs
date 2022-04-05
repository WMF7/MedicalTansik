using MedicalTansik.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace MedicalTansik.Lib
{
    public class DBUtils
    {

        private static Random _random = new Random();

        public string GenerateBassword()
        {

            List<string> passwords = new List<string>();
            string password = "";
            for (int i = 0; i < 5; ++i)
            {
                password += _random.Next(0, 10);
            }
            if (passwords.Contains(password))
            {
                GenerateBassword();
            }
            return password;
        }

        public void CreateUsersHelber()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            List<Student> students = context.students.ToList();
            foreach (Student student in students)
            {
                ApplicationUser application = new ApplicationUser()
                {
                    IsStudent = true,
                    DataConfirmed = false,
                    PasswordHash = this.GenerateBassword(),
                    PhoneNumber = student.PhoneNumber,
                    UserName = student.NatId,
                    Student = student,
                    NatId = student.NatId,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };
                try
                {
                    context.Users.Add(application);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e.EntityValidationErrors);
                }
            }
        }

        public void CreateUsersEmployees()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            string EmpUserName = "22603082601260";
            string EmpNationalID = "22603082601260";
            string phoneNum = "11111111111";

            ApplicationUser application = new ApplicationUser()
            {
                IsStudent = false,
                DataConfirmed = false,
                PasswordHash = this.GenerateBassword(),
                PhoneNumber = phoneNum,
                UserName = EmpUserName,
                Student = null,
                NatId = EmpNationalID,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            try
            {
                context.Users.Add(application);
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e.EntityValidationErrors);
            }

        }



        public static ApplicationUser GetLoggedInUser(String userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = db.Users.Include("Student").Where(u => u.Id == userId).FirstOrDefault();
            return user;
        }

        public static Student GetLoggedInStudent(String userId)
        {
            ApplicationUser user = DBUtils.GetLoggedInUser(userId);
            Student student = user.Student;
            return student;
        }

        public void CreateClinicalDesires()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            List<Desire> desires = new List<Desire>();

            Desire desire = new Desire() { Name = "الأشعة التشخيصية", Positions = 10, Active = true };
            Desire desire1 = new Desire() { Name = "الأمراض الجلدية والتناسلية", Positions = 10, Active = true };
            Desire desire2 = new Desire() { Name = "الأمراض الصدرية", Positions = 10, Active = true };
            Desire desire3 = new Desire() { Name = "الأمراض العصبية والنفسية", Positions = 10, Active = true };
            Desire desire4 = new Desire() { Name = "الأورام والطب النووي", Positions = 10, Active = true };
            Desire desire5 = new Desire() { Name = "الباثولوجيا الاكلينيكية", Positions = 10, Active = true };
            Desire desire6 = new Desire() { Name = "الباطنة العامة", Positions = 10, Active = true };
            Desire desire7 = new Desire() { Name = "التخاطب", Positions = 10, Active = true };
            Desire desire8 = new Desire() { Name = "التخدير والعناية المركزة", Positions = 10, Active = true };
            Desire desire9 = new Desire() { Name = "التوليد وأمراض النساء", Positions = 10, Active = true };
            Desire desire10 = new Desire() { Name = "الجراحة العامة", Positions = 10, Active = true };
            Desire desire11 = new Desire() { Name = "الروماتيزم والتأهيل", Positions = 10, Active = true };
            Desire desire12 = new Desire() { Name = "السمعيات", Positions = 10, Active = true };
            Desire desire13 = new Desire() { Name = "جراحة الأنف والأذن والحنجرة", Positions = 10, Active = true };
            Desire desire14 = new Desire() { Name = "جراحة الأوعية الدموية", Positions = 10, Active = true };
            Desire desire15 = new Desire() { Name = "جراحة التجميل", Positions = 10, Active = true };
            Desire desire16 = new Desire() { Name = "جراحة العظام", Positions = 10, Active = true };
            Desire desire17 = new Desire() { Name = "جراحة القلب والصدر", Positions = 10, Active = true };
            Desire desire18 = new Desire() { Name = "جراحة المخ والأعصاب", Positions = 10, Active = true };
            Desire desire19 = new Desire() { Name = "جراحة المسالك البولية", Positions = 10, Active = true };
            Desire desire20 = new Desire() { Name = "طب الأطفال", Positions = 10, Active = true };
            Desire desire21 = new Desire() { Name = "طب المناطق الحارة والجهاز الهضمي", Positions = 10, Active = true };
            Desire desire22 = new Desire() { Name = "طب وجراحة العيون", Positions = 10, Active = true };
            Desire desire23 = new Desire() { Name = "جراحة الاطفال", Positions = 10, Active = true };
            Desire desire24 = new Desire() { Name = "طب الطوارئ 	", Positions = 10, Active = true };

            desires.Add(desire);
            desires.Add(desire1);
            desires.Add(desire2);
            desires.Add(desire3);
            desires.Add(desire4);
            desires.Add(desire5);
            desires.Add(desire6);
            desires.Add(desire7);
            desires.Add(desire8);
            desires.Add(desire9);
            desires.Add(desire10);
            desires.Add(desire11);
            desires.Add(desire12);
            desires.Add(desire13);
            desires.Add(desire14);
            desires.Add(desire15);
            desires.Add(desire16);
            desires.Add(desire17);
            desires.Add(desire18);
            desires.Add(desire19);
            desires.Add(desire20);
            desires.Add(desire21);
            desires.Add(desire22);
            desires.Add(desire23);
            desires.Add(desire24);

            context.Desires.AddRange(desires);
            context.SaveChanges();

        }

        public void CreateAcademicalDesires() {

            ApplicationDbContext context = new ApplicationDbContext();

            context.Desires.AddOrUpdate(new Desire() { Name = "قسم الفارماكولوجي", IsAcademic = true });
            context.Desires.AddOrUpdate(new Desire() { Name = "قسم الفسيولوجيا الطبية", IsAcademic = true });
            context.Desires.AddOrUpdate(new Desire() { Name = "قسم الهستولوجي", IsAcademic = true });
            context.Desires.AddOrUpdate(new Desire() { Name = "قسم الصحة العامة", IsAcademic = true });
            context.Desires.AddOrUpdate(new Desire() { Name = "قسم الميكروبيولوجي", IsAcademic = true });
            context.Desires.AddOrUpdate(new Desire() { Name = "قسم التشريح", IsAcademic = true });
            context.Desires.AddOrUpdate(new Desire() { Name = "قسم الطفيليات", IsAcademic = true });
            context.Desires.AddOrUpdate(new Desire() { Name = "قسم الباثولوجيا", IsAcademic = true });
            context.Desires.AddOrUpdate(new Desire() { Name = "قسم الكيمياء الحيوية", IsAcademic = true });
            context.Desires.AddOrUpdate(new Desire() { Name = "قسم الطب الشرعي والسموم", IsAcademic = true });
            context.Desires.AddOrUpdate(new Desire() { Name = "قسم طب الأسرة", IsAcademic = true });
            context.Desires.AddOrUpdate(new Desire() { Name = "قسم التعليم الطبي", IsAcademic = true });

            context.SaveChanges();
        }


        public void CreateMedicalSubjects() {

            ApplicationDbContext context = new ApplicationDbContext();

            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "التوليد وامراض النسا" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "الباطنة العامة" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "جراحة الانف والاذن والحنجرة" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "الجراحة العامة" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "طب الاطفال" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "الفارماكولوجي" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "الفسيولوجيا الطبية-ثانية" });
			context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "الفسيولوجيا الطبية-أولى" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "الهستولوجي-ثانية" });
			context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "الهستولوجي-أولى" });
			context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "طب المجتمع" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "الميكروبيولوجي" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "التشريح-ثانية" });
			context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "التشريح-أولى" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "الطفيليات" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "الباثولوجيا" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "الكيمياء الحيوية-ثانية" });
			context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "الكيمياء الحيوية-أولى" });
            context.MedicalSubjects.AddOrUpdate(new MedicalSubject() { Name = "الطب الشرعي والسموم" });

            context.SaveChanges();

        }


        public void CreateAUser()
        {

            ApplicationDbContext context = new ApplicationDbContext();
            string EmpUserName = "hend";
            string EmpNationalID = "22603082601260";
            string phoneNum = "11111111111";
            ApplicationUser application = new ApplicationUser()
            {
                IsStudent = false,
                DataConfirmed = false,
                PasswordHash = this.GenerateBassword(),
                PhoneNumber = phoneNum,
                UserName = EmpUserName,
                Student = null,
                NatId = EmpNationalID,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
          
            context.Users.Add(application);
            context.SaveChanges();

        }




    }
}
