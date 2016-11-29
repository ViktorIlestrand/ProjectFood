using ProjectFood.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models.ViewModels.RecipeVM
{
    public class RecipeVM
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public int Portions { get; set; }
        public List<RecipeFoodItem> Ingredients { get; set; }
        public List<FoodMatching> IngredientMatch { get; set; }
        public double MatchPercentage { get; set; }
        public string Instruction { get; set; }
        public string Picture { get; set; }
        public int CookingTime { get; set; }

        public RecipeVM(int recipeId, string title, int portions, List<RecipeFoodItem> ingredients, List<FoodMatching> ingredientmatch, 
            double matchpercentage, string instruction, string picture, int cookingtime)
        {
            RecipeId = recipeId;
            Title = title;
            Portions = portions;
            Ingredients = ingredients;
            IngredientMatch = ingredientmatch;
            MatchPercentage = matchpercentage;
            Instruction = instruction;
            Picture = picture;
            CookingTime = cookingtime;
        }
    }
}
