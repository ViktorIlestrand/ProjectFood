using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models.ViewModels.AdminVM
{
    public class AddRecipeVM
    {
        public string Title { get; set; }
        public string Instructions { get; set; }
        public int Portions { get; set; }
        public int CookingTime { get; set; }
        public string ImageUrl { get; set; }

        public AddRecipeVM()
        {
       
        }
        
    }
}
