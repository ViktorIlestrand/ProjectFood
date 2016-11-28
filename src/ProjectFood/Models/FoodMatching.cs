using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models
{
    public class FoodMatching
    {
        public bool UserHasItem { get; set; }
        public string FoodItem { get; set; }
        public int RecipeId { get; set; }

        public FoodMatching(bool userhasitem, string fooditem, int recipeid)
        {
            UserHasItem = userhasitem;
            FoodItem = fooditem;
            RecipeId = recipeid;
        }
    }
}
