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
    public class DashboardController : Controller
    {
        // GET: Dashboard
        [Authorize]
        public ActionResult Index()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //Tansik t = new Tansik();
                //DBUtils s = new DBUtils();
                //List<InstantResult> re = new List<InstantResult>();
                //List<StudentDesire> r = db.StudentDesires.ToList();
                //re = t.DoIt(r);
                //return View(re);
                return null;
            }
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
