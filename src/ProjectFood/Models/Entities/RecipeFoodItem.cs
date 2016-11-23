using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class RecipeFoodItem
    {
        public int Id { get; set; }
        public int FoodItemId { get; set; }
        public double Quantity { get; set; }
        public int Measurement { get; set; }

        public virtual FoodItem FoodItem { get; set; }
    }
}
