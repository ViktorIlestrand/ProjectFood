using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class UserFoodItem
    {
        public int Id { get; set; }
        public int FoodItemId { get; set; }
        public DateTime? Expires { get; set; }
        public int UserId { get; set; }

        public virtual FoodItem FoodItem { get; set; }
        public virtual User User { get; set; }
    }
}
