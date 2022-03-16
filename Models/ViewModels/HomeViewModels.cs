using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalTansik.Models.ViewModels
{
	public class SaveDesiresAjax
	{
		public string Action { set; get; }
		public int[] Data {set; get;} //desires 

	}
}