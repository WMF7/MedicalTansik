using MedicalTansik.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Web;

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
			List< Student > students = context.students.ToList();
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
				};
				try
				{
					context.Users.Add(application);
					context.SaveChanges();
				} catch(DbEntityValidationException e)
				{
					Console.WriteLine(e.EntityValidationErrors);
				}
			}
		}

	}
}