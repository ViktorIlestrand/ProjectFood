using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models.ViewModels.User
{
    public class RegisterVM
    {
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-postadress")]
        [Required(ErrorMessage = "Ange E-postadress")]
        public string Email { get; set; }

        [Display(Name ="Användarnamn")]
        [Required(ErrorMessage ="Ange ett användarnamn")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ange Lösenord")]
        [Display(Name = "Lösenord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Lösenord")]
        [Compare("Password", ErrorMessage ="Lösenorden matchar inte")]
        public string ComparedPassword { get; set; }
    }
}
