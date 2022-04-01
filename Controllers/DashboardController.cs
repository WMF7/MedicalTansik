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

        // GET: Dashboard/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dashboard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


    }
}
