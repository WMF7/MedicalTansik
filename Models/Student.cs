using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace MedicalTansik.Models
{

    public class Student
	{
		[Key]
        public long Id {set; get;}
		[Required]
        public string NatId { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		[DataType(DataType.PhoneNumber)]
		[Display(Name = "رقم الهاتف")]
		public string PhoneNumber { set; get; }
		[Required]
		public string Gender {set; get;}
		[Required]
		public string Nationality {set; get;}
		public string City {set; get;}
		public string Governate { set; get; }
		[Required]
		public string BirthDay {set; get;}
		[Required]
		public string BirthMonth {set; get;}
		[Required]
		public string BirthYear {set; get;}
        public string Address {set; get;}
		[Required]
        public string Total {set; get;}
		[Required]
		public string Percentage {set; get;}
		//[Required]
		public GradeYear GradeYear {set; get;}
		public String Rank { set; get; }
	}

	
}
