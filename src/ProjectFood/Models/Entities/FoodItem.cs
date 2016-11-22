using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class FoodItem
    {
        public FoodItem()
        {
            Ingredient = new HashSet<Ingredient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Ingredient> Ingredient { get; set; }
    }
}
