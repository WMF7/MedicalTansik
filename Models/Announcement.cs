using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace MedicalTansik.Models
{
	public class Announcment
	{
		[Key]
		public long Id {set; get;}
		[Required]
		public GradeYear GradeYear {set; get;}
	}
}
