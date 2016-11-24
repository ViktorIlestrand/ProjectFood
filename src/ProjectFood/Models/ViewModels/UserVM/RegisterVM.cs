using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models.ViewModels.UserVM
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

        [Required(ErrorMessage = "Ange lösenord, måste vara minst 6 tecken och innehålla minst en stor bokstav")]
        [Display(Name = "Lösenord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Upprepa Lösenord")]
        [Compare("Password", ErrorMessage ="Lösenorden matchar inte")]
        [DataType(DataType.Password)]
        public string ComparedPassword { get; set; }
    }
}
