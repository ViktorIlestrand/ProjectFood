using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class UserFoodItem
    {
        public int Id { get; set; }
        public int FoodItemId { get; set; }
        public DateTime? Expires { get; set; }
        public int KitchenStorageId { get; set; }

        public virtual FoodItem FoodItem { get; set; }
        public virtual KitchenStorage KitchenStorage { get; set; }
    }
}
