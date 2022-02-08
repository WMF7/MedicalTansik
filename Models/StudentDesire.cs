using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace MedicalTansik.Models
{
	public class StudentDesire
	{
		[Key]
		public long Id {set; get;}
		[Required]
		public Student Student { set; get; }
		[Required]
		public Desire Desire {set; get;}
		[Required]
		public int rank {set; get;}
		public Announcment Announcment {set; get;}
	}
}
