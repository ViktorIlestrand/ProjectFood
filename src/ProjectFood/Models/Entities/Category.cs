using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class Category
    {
        public Category()
        {
            FoodItemCategory = new HashSet<FoodItemCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FoodItemCategory> FoodItemCategory { get; set; }
    }
}
