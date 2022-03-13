using MedicalTansik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalTansik.Lib
{
	public class Tansik
	{

		private Dictionary<String, List<Student>> Result = new Dictionary<String, List<Student>>();
		private Dictionary<Student, List<Desire>> studentDesiresDict = new Dictionary<Student, List<Desire>>();

		public Tansik(List<StudentDesire> studentDesires)
		{
			ApplicationDbContext db = new ApplicationDbContext();
			List<Desire> desires = db.Desires.ToList();


			//fill the result dictionary with empty values;
			foreach (Desire desire in desires)
			{
				this.Result.Add(desire.Name, new List<Student>());
			}

			//organize the students and desires
			foreach (StudentDesire studentDesire in studentDesires)
			{
				if (studentDesiresDict.ContainsKey(studentDesire.Student))
				{
					studentDesiresDict[studentDesire.Student].Add(studentDesire.Desire);
				}
				else
				{
					studentDesiresDict.Add(studentDesire.Student, new List<Desire> { studentDesire.Desire });
				}
			}

		}

		public void DoIt()
        {
            foreach (KeyValuePair < Student, List<Desire>> entry in studentDesiresDict){
                SkkenStudent(entry.Key, entry.Value);
            }
        }

		private void SkkenStudent(Student student, List<Desire> desires)
		{
			foreach (Desire desire in desires) {
				if(MatchDesireWithStudent(student, desire)) {
					break;
				}
			}
			
		}

		private bool MatchDesireWithStudent(Student student, Desire desire) {
			if (StudentEligibleForDesire(student, desire)) {
				if (IsNotFull(desire)) {
					this.Result[desire.Name].Add(student);
					return true;
				} else {
					TryExclude(desire, student);
				}
			}
			return false;
		}

		private bool IsNotFull(Desire desire)
		{
			return this.Result[desire.Name].Count < desire.Positions;
		}

		private bool StudentEligibleForDesire(Student student, Desire desire)
		{
			//FIXME
			string grad = db.gradeStudentsInMedicalSubjects.Include("MedicalSubject").Include("Student").Where(a => a.Med.Id == studentDesire.Desire.MedicalSubject.Id).FirstOrDefault().grade;
			if (desire.IsAcademic == true)
			{
				if (grad == "جيد جدا")
				{
					return true;
				}
				else return false;
			}
			else //not Academic
			{
				if (grad == "جيد")
				{
					return true;
				}
				else return false;
			}

			return true;
		}

		public void TryExclude(Desire desire, Student excluderStudent) {
			foreach (Student existingStudent in new List<Student>(this.Result[desire.Name])) {
				if(Convert.ToInt32(excluderStudent.Total) > Convert.ToInt32(existingStudent.Total))
				{
					ReplaceStudents(desire, existingStudent, excluderStudent);
				}
			}
		}

		private void ReplaceStudents(Desire desire, Student existingStudent, Student excluderStudent)
		{
			List<Desire> existingStudentDesires
				= GetSpecificStudentDesires(existingStudent);
			int existingStudentIndex = this.Result[desire.Name].IndexOf(existingStudent);
			this.Result[desire.Name][existingStudentIndex] = excluderStudent;
			this.SkkenStudent(existingStudent, existingStudentDesires);

		}

		private List<Desire> GetSpecificStudentDesires(Student student)
		{
			//TODO: this is trash, find a way to optmize database access;
			ApplicationDbContext db = new ApplicationDbContext();
			List<Desire> desires = new List<Desire>();
			List<StudentDesire> studentDesires = db.StudentDesires.Include("Desire"). Where(sd => sd.Student.Id == student.Id ).ToList();
			foreach (StudentDesire studentDesire in studentDesires)
			{
				if(!desires.Contains(studentDesire.Desire))
				{
					desires.Add(studentDesire.Desire);
				}
			}
			return desires;
		}

		public Dictionary<String, List<Student>> GetResults()
		{
			return this.Result;
		}


	} //End Class


	

	public class TansikUtils
	{
		public static bool StudentDidDoTansik(Student student)
		{
            long id = student.Id;
            ApplicationDbContext db = new ApplicationDbContext();
            List<StudentDesire> studentDesires = db.StudentDesires.Include("Student").Where(sd => sd.Student.Id == id).ToList();
            if (studentDesires.Count != 0)
            {
                return true;
            }
            return false;
		}
		
	}


	public class InstantResult
	{
		public Desire Desire { set; get; }
		public int Number { set; get; }
		public List<Student> Students { set; get; }
	}
}
