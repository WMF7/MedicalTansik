using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace MedicalTansik.Models
{
	public class GradeYear
	{
		[Key]
		public long Id {set; get;}
		[Required]
		public string year {set; get;}
	}
}
