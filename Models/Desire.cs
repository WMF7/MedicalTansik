using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace MedicalTansik.Models
{
	public class Desire
	{
		[Key]
		public long Id {set; get;}
		[Required]
		public string Name {set; get;}
		public MedicalSubject MedicalSubject {set; get;}
		public int Positions {set; get	;} //number of open positions;
		public bool Active {set; get;} 
		public bool IsAcademic { set; get; } 
	}
}
