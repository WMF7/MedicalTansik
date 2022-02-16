using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalTansik.Models.ViewModels
{
   

    public class LoginViewModel
    {
        [Required(ErrorMessage = "required")]
        [Display(Name = "الرقم القومي")]
        [MinLength(14, ErrorMessage = "الرقم القومي يجب ان يكون 14 رقم")]
        [MaxLength(14, ErrorMessage = "الرقم القومي يجب ان يكون 14 رقم")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "required")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

    }

  

}
