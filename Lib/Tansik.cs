using MedicalTansik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalTansik.Lib
{
	public class Tansik
	{
		ApplicationDbContext db = new ApplicationDbContext();
		public List<InstantResult> DoIt(List<StudentDesire> tempstus)
		{
             tempstus =db.StudentDesires.Include("Desire").Include("Student").ToList();
			
			List<Student> stu=new List<Student>();
			foreach (var t in tempstus)
			{
				stu.Add((from m in db.students where m.Id == t.Student.Id select m).FirstOrDefault());
			}
			
			List<InstantResult> tr = new List<InstantResult>();

			foreach (var k in tempstus)
			{
				var des = (db.Desires.Where(a => a.Id == k.Desire.Id).FirstOrDefault());
				InstantResult temp = new InstantResult() { Desire = des, Number = stu.Count, Students = stu };
				tr.Add(temp);
			}
			
			return tr;
		}
	}


	public class InstantResult
	{
		public Desire Desire { set; get; }
		public int Number { set; get; }
		public List<Student> Students { set; get; }
		
	}
}