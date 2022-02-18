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

		public string Test()
		{
			DBUtils db = new DBUtils();
			db.CreateUsersHelber();
			return "done";
		}


	}
}
