using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class Recipe
    {
        public Recipe()
        {
            RecipeCategory = new HashSet<RecipeCategory>();
            RecipeIngredient = new HashSet<RecipeIngredient>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public int Portions { get; set; }
        public int? CookingTime { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<RecipeCategory> RecipeCategory { get; set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredient { get; set; }
    }
}
