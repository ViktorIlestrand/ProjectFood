using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models.ViewModels.User
{
    public class LoginVM
    {
        //[EmailAddress]
        //[DataType(DataType.EmailAddress)]
        //[Required(ErrorMessage = "Ange E-postadress")]
        //public string Email { get; set; }

        [Display(Name = "Användarnamn")]
        [Required(ErrorMessage = "Ange användarnamn")]
        public string UserName { get; set; }

        [Display(Name = "Lösenord")]
        [Required(ErrorMessage = "Ange lösenord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
