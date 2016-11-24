using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class FoodItemCategory
    {
        public int Id { get; set; }
        public int FoodItemId { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual FoodItem FoodItem { get; set; }
    }
}
