using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalTansik.Models
{
	public class StudentSubjectDegree
	{
		[Key]
		public long Id { set; get; }
		public Student Student { set; get; }
		public MedicalSubject MedicalSubject { set; get; }
		public string Takdeer { set; get; }
	}
}