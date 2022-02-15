using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;

namespace MedicalTansik.Models
{
	public class MedicalSubject
	{
		[Key]
		public long Id {set; get;}
		[Required]
		public string Name {set; get;}
	}
}
