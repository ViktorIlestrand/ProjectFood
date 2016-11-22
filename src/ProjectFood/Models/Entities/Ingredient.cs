using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            RecipeIngredient = new HashSet<RecipeIngredient>();
            UserIngredient = new HashSet<UserIngredient>();
        }

        public int Id { get; set; }
        public int Ingredient1 { get; set; }
        public int Measurement { get; set; }
        public double Quantity { get; set; }

        public virtual ICollection<RecipeIngredient> RecipeIngredient { get; set; }
        public virtual ICollection<UserIngredient> UserIngredient { get; set; }
        public virtual FoodItem Ingredient1Navigation { get; set; }
    }
}
