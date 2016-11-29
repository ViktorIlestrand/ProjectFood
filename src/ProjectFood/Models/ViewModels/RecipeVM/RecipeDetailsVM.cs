using ProjectFood.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models.ViewModels.RecipeVM
{
    public class RecipeDetailsVM
    {
        public string Title { get; set; }
        public string Instructions { get; set; }
        public int Portions { get; set; }
        public int CookingTime { get; set; }
        public string ImgUrl { get; set; }
        public List<RecipeFoodItem> Ingredients { get; set; }

        public RecipeDetailsVM(string title, string instr, int port, int ctime, string img, List<RecipeFoodItem> ingredients)
        {
            Title = title;
            Instructions = instr;
            Portions = port;
            CookingTime = ctime;
            ImgUrl = img;
            Ingredients = ingredients;
        }
    }
}
