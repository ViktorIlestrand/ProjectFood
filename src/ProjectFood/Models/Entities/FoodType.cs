using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class FoodType
    {
        public FoodType()
        {
            FoodItem = new HashSet<FoodItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FoodItem> FoodItem { get; set; }
    }
}
