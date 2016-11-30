using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models.ViewModels.RecipeVM
{
    public class AllUserRecipesVM
    {
        public List<RecipeVM> expiringRecipes { get; set; }
        public List<RecipeVM> matchedRecipes { get; set; }
    }
}
