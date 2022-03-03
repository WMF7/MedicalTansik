using MedicalTansik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalTansik.Lib
{
	public class Tansik
	{

		/*
		  How tansik works - 
		  we hve a list of student desires 
		 */
		public List<InstantResult> DoIt(List<StudentDesire> studentDesires)
		{
			
			return null;
		}
		
	}


	public class InstantResult
	{
		public Desire Desire { set; get; }
		public int Number { set; get; }
		public List<Student> Students { set; get; }
	}
}
