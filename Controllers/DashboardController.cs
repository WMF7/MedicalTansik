using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalTansik.Lib;
using MedicalTansik.Models;
using Microsoft.AspNet.Identity;


namespace MedicalTansik.Controllers
{
    public class viewmodel
    {
        public IEnumerable<Student> stu { get; set; }
        public IEnumerable<StudentDesire> std { get; set; }
    }
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        public ActionResult Index()
        {
           if (DBUtils.GetLoggedInUser(User.Identity.GetUserId()).IsStudent)
            {
                return RedirectToAction("Login", "Account");
            }
                var sts = db.students.ToList();
                ViewData["studesire"] = db.StudentDesires.ToList();
                var sdlist= db.StudentDesires.Include("Desire").ToList().OrderBy(a=>a.rank);
                dynamic m = new System.Dynamic.ExpandoObject();
                m.stu = sts;
                m.std = sdlist;
                return View(m);
           
        }


        public string AddNewUser()
        {
            DBUtils db = new DBUtils();
            db.CreateUsersEmployees();
            return "Added";
        }

        [HttpGet]
        public ActionResult show(int id)
        {
            var studentvar = db.StudentSubjectDegrees.Include("MedicalSubject").Where(a => a.Student.Id == id).ToList();
            if (studentvar != null)
            {
                return View(studentvar);
            }
            else { return RedirectToAction("Index"); }
        }

        // GET: Dashboard/Create
        public ActionResult Create()
        {
            return View();
        }

         // POST: Dashboard/delete
        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                // TODO: delete all student desires 
                db.StudentDesires.RemoveRange(db.StudentDesires.ToList());
                db.SaveChanges();
                return View();
            }
            catch
            {
                return View();
            }
        }


        [AllowAnonymous]
        public  ActionResult PublicRealTimeTansik()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Tansik tansik = new Tansik(db.StudentDesires.Include("Desire").Include("Desire.MedicalSubject").Include("Student").ToList());
            tansik.DoIt();
            Dictionary<String, List<Student>> results = tansik.GetResults();
            ViewBag.results = results;
            return View("PublicRealTimeTansik"); 
		}
    }
}
