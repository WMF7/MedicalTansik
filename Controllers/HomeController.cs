using MedicalTansik.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MedicalTansik.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using MedicalTansik.Models.ViewModels;

namespace MedicalTansik.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{

		private ApplicationDbContext db = new ApplicationDbContext();

		public ActionResult Index()
		{
			if(!DBUtils.GetLoggedInUser(User.Identity.GetUserId()).DataConfirmed) 
			{
				return RedirectToAction("ConfirmStudentData", "Home");
			}
			Student student = DBUtils.GetLoggedInStudent(User.Identity.GetUserId());

			if (TansikUtils.StudentDidDoTansik(student))
            {
				ApplicationDbContext db = new ApplicationDbContext();
				List<StudentDesire> studentDesires = db.StudentDesires.Include("Desire").Include("Student").Where(sd => sd.Student.Id == student.Id).ToList();
				ViewBag.Student = student;
				ViewBag.StudentDesires = studentDesires;
				return View("PrintTansikInfo");
            }
            List<Desire> desires = this.db.Desires.ToList();
			ViewBag.Desires = desires;
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		
		public ActionResult RealTimeTansik()
		{
			ApplicationDbContext db = new ApplicationDbContext();
			Tansik tansik = new Tansik(db.StudentDesires.Include("Desire").Include("Desire.MedicalSubject").Include("Student").ToList());
			tansik.DoIt();
			Dictionary<String, List<Student>> results =tansik.GetResults();
			ViewBag.results = results;
			return View();
		}

		[HttpGet]
		public ActionResult ConfirmStudentData()
		{
			var userId = User.Identity.GetUserId();
			ApplicationUser user = DBUtils.GetLoggedInUser(userId);
			if(user.DataConfirmed)
			{
				return RedirectToAction("Index", "Home");
			}
			ViewBag.Student = user.Student;
			return View();
		}

		[HttpPost]
		public ActionResult ConfirmStudentDataPost()
		{
			using (var db = new ApplicationDbContext()) 
			{
				string userId = User.Identity.GetUserId();
				ApplicationUser user = db.Users.Where(u => u.Id == userId).FirstOrDefault();
				{
					if (!user.DataConfirmed)
					{
						user.DataConfirmed = true;
						if (db.SaveChanges() > 0)
						{
							//TODO(walid): flash a success message using tempdata;
							return RedirectToAction("Index", "Home");
						}
					}
				}

			}
			return RedirectToAction("Index", "Home");
		}


		[HttpPost]
        public ActionResult SaveDesires(SaveDesiresAjax req)
        {
			//The result that we return from this method "SaveDesires" to the AJAX call;
			//it is a dictionary because it's convertable to JSON, both share the same concept of key-value storage.
			Dictionary<string, string> result = new Dictionary<string, string>();
			ApplicationDbContext db = new ApplicationDbContext();
			Student student = DBUtils.GetLoggedInStudent(User.Identity.GetUserId());
			/* 
			 * NOTE(walid): this is a very important, we can't use the student object that we
			 * obtained from the function "GetLoggedInStudent" directly as a property to create a new StudentDesire
			 * object, that will simply recreate the student, student HAVE TO BE fetched from the same
			 * dbContext.
			*/
			student = db.students.Find(student.Id); 
			Announcment announcment = db.Announcments.Find(1);


			if (TansikUtils.StudentDidDoTansik(DBUtils.GetLoggedInStudent(User.Identity.GetUserId())))
			{
				result.Add("status", "error");
				result.Add("message", "took the tansik before");
				return Json(result);
			}

			if (req.Action == "ADD_DESIRES")
			{
				if(req.Data.Length == 0)
				{
					result.Add("status", "error");
					result.Add("message", "الرغبات مش موجودة");
					return Json(result);
				}

				foreach (int i in req.Data)
				{
					Desire desire = db.Desires.Find(i);
                    db.Entry(desire).Reference("MedicalSubject").Load();
					string takdeer = Tansik.ExtractTakdeerFromDesire(desire, student).Trim();
					if (desire.IsAcademic)
					{
						if (!takdeer.Equals("جيد جدا") && !takdeer.Equals("ممتاز"))
						{
							result.Add("status", "error");
							result.Add("message", "انت غير مؤهل لاختيار هذه الرغبة");
							return Json(result);
						}
					} else {
						if (takdeer.Equals("مقبول"))
						{
							result.Add("status", "error");
							result.Add("message", "انت غير مؤهل لاختيار هذه الرغبة");

							return Json(result);
						}
					}

                }

				foreach (int i in req.Data)
				{
					Desire desire = db.Desires.Find(i);
					db.StudentDesires.Add(new StudentDesire() { Announcment = announcment, Student = student, Desire = desire, rank = Array.IndexOf(req.Data, i) + 1 }); ;
					db.SaveChanges();
				}
				result.Add("status", "success");
				result.Add("message", "تم حفظ الرغبات بنجاح.");
				return Json(result);
			} 
			
            result.Add("status", "error");
            return Json(result);
        }


		public ActionResult Info() {

			return View();
		}




		//[AllowAnonymous]
  //      public string Test()
		//{
			
		//	ApplicationDbContext db = new ApplicationDbContext();
		//	String resultString = "";
		//	Tansik tansik = new Tansik(db.StudentDesires.Include("Desire").Include("Desire.MedicalSubject").Include("Student").ToList());
		//	tansik.DoIt();
		//	Dictionary<String, List<Student>> results =tansik.GetResults();
		//	foreach(KeyValuePair<String, List<Student>> entry in results)
		//	{
		//		if (entry.Value.Count == 0) continue; //not printing the empty desires;
		//		resultString += "<h1> " + entry.Key + "</h1>";
		//		resultString += "<br>";
		//		foreach(Student student in entry.Value)
		//		{
		//			resultString += student.Name;
		//			resultString += "<br>";
		//		}
		//		resultString += "<br>";
		//		resultString += "<br>";
		//		resultString += "<hr>";
			

		//	}
		//	return resultString;
		//}

		[AllowAnonymous]
		public string Test(int id)
		{
            ApplicationDbContext db = new ApplicationDbContext();
			Desire desire = db.Desires.Find(Convert.ToInt32(Request.QueryString.Get("desire"))) ;
            db.Entry(desire).Reference(d => d.MedicalSubject).Load();
            Student student = db.students.Find(id);
            string result = Tansik.ExtractTakdeerFromDesire(desire, student);
            return student.Name + " " + result;
		}

	}
}
