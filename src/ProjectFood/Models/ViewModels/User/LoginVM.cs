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

        [Required(ErrorMessage = "Ange användarnamn")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Ange lösenord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
