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
                    return TryExclude(desire, student);
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
            if (desire.Id == 37) //تعليم طبي 
            {
                return true;
            }
            string takdeer = Tansik.ExtractTakdeerFromDesire(desire, student);
            if (desire.IsAcademic)
            {
                if (!takdeer.Equals(  "جيد جدا") && !takdeer.Equals(  "ممتاز"))
                {
                    return false;
                }
            } else {
                if (takdeer.Equals(  "مقبول"))
                {
                    return true;
                }
            }

            return true;
        }

        public bool TryExclude(Desire desire, Student excluderStudent) {

			foreach (Student existingStudent in new List<Student>(this.Result[desire.Name]))
			{
				if (Convert.ToDouble(excluderStudent.Total) > Convert.ToDouble(existingStudent.Total))
				{
					ReplaceStudents(desire, existingStudent, excluderStudent);
                    return true;
                } else if (Convert.ToDouble(excluderStudent.Total) == Convert.ToDouble(existingStudent.Total))
                {
                    if(this.DoesHaveGreaterDegreeInSubject(excluderStudent, existingStudent, desire)) {
						ReplaceStudents(desire, existingStudent, excluderStudent);
                        return true;
                    }
                } else if(Convert.ToInt32(excluderStudent.GradeYear) < Convert.ToInt32(existingStudent.GradeYear))
                {
					ReplaceStudents(desire, existingStudent, excluderStudent);
                    return true;
                }
            }
            return false;
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
            List<StudentDesire> studentDesires = db.StudentDesires.Include("Desire").Include("Desire.MedicalSubject"). Where(sd => sd.Student.Id == student.Id ).ToList();
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





        public static string ExtractTakdeerFromDesire(Desire desire, Student student)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MedicalSubject medicalSubject = desire.MedicalSubject;
            if (medicalSubject == null)
			{
                return "ممتاز";
			}
            if (!medicalSubject.Name.Contains("هيستولوجي"))
            {
                string takdir = db.StudentSubjectDegrees
                    .Where(ssd => ssd.MedicalSubject.Id == medicalSubject.Id && ssd.Student.Id == student.Id)
                    .FirstOrDefault()
                    .Takdeer.Trim();
                return takdir;
            }
            else
            {
                if (medicalSubject.Name.Contains("هيستولوجي"))
                {
                    String year1 = db.StudentSubjectDegrees
                        .Where(ssd => ssd.MedicalSubject.Id == 10 && ssd.Student.Id == student.Id)
                        .FirstOrDefault()
						.Takdeer.Trim();
                    String year2 = db.StudentSubjectDegrees
                        .Where(ssd => ssd.MedicalSubject.Id == 9 && ssd.Student.Id == student.Id)
                        .FirstOrDefault()
                        .Takdeer.Trim();

                    //جيد
                    if (year1.Equals( "جيد") && year2.Equals( "جيد"))
                    {
                        return "جيد";
                    }
                    else if ((year1.Equals( "جيد") && year2.Equals( "جيد جدا")) || (year2.Equals( "جيد") && year1.Equals( "جيد جدا")) )
                    {
                        return "جيد";
                    }
					else if ((year1.Equals( "جيد") && year2.Equals( "ممتاز")) || (year2.Equals( "جيد") && year1.Equals( "ممتاز")))
                    {
                        return "جيد جدا";
                    }
					

                    //ممتاز
                    else if (year1.Equals( "ممتاز") && year2.Equals( "ممتاز"))
                    {
                        return "ممتاز";
                    }
                    else if ((year1.Equals( "ممتاز") && year2.Equals( "جيد جدا")) || (year2.Equals( "ممتاز") && year1.Equals( "جيد جدا")))
                    {
                        return "جيد جدا";
                    }

                    //جيد جدا
                    else if (year1.Equals("جيد جدا") && year2.Equals("جيد جدا"))
                    {
                        return "جيد جدا";
                    }

                    //مقبول
                    else if (year1.Equals( "مقبول") && year2.Equals( "مقبول"))
                    {
                        return "مقبول";
                    }
                    else if ((year1.Equals( "مقبول") && year2.Equals( "جيد")) || (year2.Equals( "مقبول") && year1.Equals( "جيد")))
                    {
                        return "مقبول";
                    }
                    else if ((year1.Equals( "مقبول") && year2.Equals( "جيد جدا")) || (year2.Equals( "مقبول") && year1.Equals("جيد جدا")))
                    {
                        return "جيد";
                    }
                    else if ((year1.Equals( "مقبول") && year2.Equals( "ممتاز")) || (year2.Equals( "مقبول") && year1.Equals( "ممتاز")))
                    {
                        return "جيد";
                    }

                } // هيستولوجي
                if (medicalSubject.Name.Contains("فسيولوجيا"))
                {
                    String year1 = db.StudentSubjectDegrees
                        .Where(ssd => ssd.MedicalSubject.Id == 8 && ssd.Student.Id == student.Id)
                        .FirstOrDefault()
                        .Takdeer.Trim();
                    String year2 = db.StudentSubjectDegrees
                        .Where(ssd => ssd.MedicalSubject.Id == 7 && ssd.Student.Id == student.Id)
                        .FirstOrDefault()
                        .Takdeer.Trim();

                                        //جيد
                    if (year1.Equals( "جيد") && year2.Equals( "جيد"))
                    {
                        return "جيد";
                    }
                    else if ((year1.Equals( "جيد") && year2.Equals( "جيد جدا")) || (year2.Equals( "جيد") && year1.Equals( "جيد جدا")) )
                    {
                        return "جيد";
                    }
					else if ((year1.Equals( "جيد") && year2.Equals( "ممتاز")) || (year2.Equals( "جيد") && year1.Equals( "ممتاز")))
                    {
                        return "جيد جدا";
                    }
					

                    //ممتاز
                    else if (year1.Equals( "ممتاز") && year2.Equals( "ممتاز"))
                    {
                        return "ممتاز";
                    }
                    else if ((year1.Equals( "ممتاز") && year2.Equals( "جيد جدا")) || (year2.Equals( "ممتاز") && year1.Equals( "جيد جدا")))
                    {
                        return "جيد جدا";
                    }

                    //جيد جدا
                    else if (year1.Equals("جيد جدا") && year2.Equals("جيد جدا"))
                    {
                        return "جيد جدا";
                    }

                    //مقبول
                    else if (year1.Equals( "مقبول") && year2.Equals( "مقبول"))
                    {
                        return "مقبول";
                    }
                    else if ((year1.Equals( "مقبول") && year2.Equals( "جيد")) || (year2.Equals( "مقبول") && year1.Equals( "جيد")))
                    {
                        return "مقبول";
                    }
                    else if ((year1.Equals( "مقبول") && year2.Equals( "جيد جدا")) || (year2.Equals( "مقبول") && year1.Equals("جيد جدا")))
                    {
                        return "جيد";
                    }
                    else if ((year1.Equals( "مقبول") && year2.Equals( "ممتاز")) || (year2.Equals( "مقبول") && year1.Equals( "ممتاز")))
                    {
                        return "جيد";
                    }

                }  // فسيولوجي
                if (medicalSubject.Name.Contains("تشريح"))
                {
                    String year1 = db.StudentSubjectDegrees
                        .Where(ssd => ssd.MedicalSubject.Id == 14 && ssd.Student.Id == student.Id)
                        .FirstOrDefault()
                        .Takdeer.Trim();
                    String year2 = db.StudentSubjectDegrees
                        .Where(ssd => ssd.MedicalSubject.Id == 13 && ssd.Student.Id == student.Id)
                        .FirstOrDefault()
                        .Takdeer.Trim();

                                        //جيد
                    if (year1.Equals( "جيد") && year2.Equals( "جيد"))
                    {
                        return "جيد";
                    }
                    else if ((year1.Equals( "جيد") && year2.Equals( "جيد جدا")) || (year2.Equals( "جيد") && year1.Equals( "جيد جدا")) )
                    {
                        return "جيد";
                    }
					else if ((year1.Equals( "جيد") && year2.Equals( "ممتاز")) || (year2.Equals( "جيد") && year1.Equals( "ممتاز")))
                    {
                        return "جيد جدا";
                    }
					

                    //ممتاز
                    else if (year1.Equals( "ممتاز") && year2.Equals( "ممتاز"))
                    {
                        return "ممتاز";
                    }
                    else if ((year1.Equals( "ممتاز") && year2.Equals( "جيد جدا")) || (year2.Equals( "ممتاز") && year1.Equals( "جيد جدا")))
                    {
                        return "جيد جدا";
                    }

                    //جيد جدا
                    else if (year1.Equals("جيد جدا") && year2.Equals("جيد جدا"))
                    {
                        return "جيد جدا";
                    }

                    //مقبول
                    else if (year1.Equals( "مقبول") && year2.Equals( "مقبول"))
                    {
                        return "مقبول";
                    }
                    else if ((year1.Equals( "مقبول") && year2.Equals( "جيد")) || (year2.Equals( "مقبول") && year1.Equals( "جيد")))
                    {
                        return "مقبول";
                    }
                    else if ((year1.Equals( "مقبول") && year2.Equals( "جيد جدا")) || (year2.Equals( "مقبول") && year1.Equals("جيد جدا")))
                    {
                        return "جيد";
                    }
                    else if ((year1.Equals( "مقبول") && year2.Equals( "ممتاز")) || (year2.Equals( "مقبول") && year1.Equals( "ممتاز")))
                    {
                        return "جيد";
                    }


                } // تشريح
                if (medicalSubject.Name.Contains("الحيوية"))
                {
                    String year1 = db.StudentSubjectDegrees
                        .Where(ssd => ssd.MedicalSubject.Id == 18 && ssd.Student.Id == student.Id)
                        .FirstOrDefault()
                        .Takdeer.Trim();
                    String year2 = db.StudentSubjectDegrees
                        .Where(ssd => ssd.MedicalSubject.Id == 17 && ssd.Student.Id == student.Id)
                        .FirstOrDefault()
                        .Takdeer.Trim();

                                        //جيد
                    if (year1.Equals( "جيد") && year2.Equals( "جيد"))
                    {
                        return "جيد";
                    }
                    else if ((year1.Equals( "جيد") && year2.Equals( "جيد جدا")) || (year2.Equals( "جيد") && year1.Equals( "جيد جدا")) )
                    {
                        return "جيد";
                    }
					else if ((year1.Equals( "جيد") && year2.Equals( "ممتاز")) || (year2.Equals( "جيد") && year1.Equals( "ممتاز")))
                    {
                        return "جيد جدا";
                    }
					

                    //ممتاز
                    else if (year1.Equals( "ممتاز") && year2.Equals( "ممتاز"))
                    {
                        return "ممتاز";
                    }
                    else if ((year1.Equals( "ممتاز") && year2.Equals( "جيد جدا")) || (year2.Equals( "ممتاز") && year1.Equals( "جيد جدا")))
                    {
                        return "جيد جدا";
                    }

                    //جيد جدا
                    else if (year1.Equals("جيد جدا") && year2.Equals("جيد جدا"))
                    {
                        return "جيد جدا";
                    }

                    //مقبول
                    else if (year1.Equals( "مقبول") && year2.Equals( "مقبول"))
                    {
                        return "مقبول";
                    }
                    else if ((year1.Equals( "مقبول") && year2.Equals( "جيد")) || (year2.Equals( "مقبول") && year1.Equals( "جيد")))
                    {
                        return "مقبول";
                    }
                    else if ((year1.Equals( "مقبول") && year2.Equals( "جيد جدا")) || (year2.Equals( "مقبول") && year1.Equals("جيد جدا")))
                    {
                        return "جيد";
                    }
                    else if ((year1.Equals( "مقبول") && year2.Equals( "ممتاز")) || (year2.Equals( "مقبول") && year1.Equals( "ممتاز")))
                    {
                        return "جيد";
                    }


                } // كيميا حيوية
                return null;
            }
        }


		public bool IsStudentTakdeerEligible(Desire desire, Student student)
		{
			return false;
		}

        public bool DoesHaveGreaterDegreeInSubject(Student s1, Student s2, Desire desire)
        {
            Dictionary<string, int> map = new Dictionary<string, int>()
            {
                ["مقبول"] = 1,
                ["جيد"] = 2,
                ["جيد جدا"] = 3,
                ["ممتاز"] = 4
            };
            string s1Takdeer = Tansik.ExtractTakdeerFromDesire(desire, s1);
            string s2Takdeer = Tansik.ExtractTakdeerFromDesire(desire, s2);
            if (map[s1Takdeer] > map[s2Takdeer])
            {
                return true;
            }
            else
            {
                return false;
            }
					
	}
	
	
} //End// Class


	

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
