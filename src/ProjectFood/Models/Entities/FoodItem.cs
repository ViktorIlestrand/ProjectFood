using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class FoodItem
    {
        public FoodItem()
        {
            FoodItemCategory = new HashSet<FoodItemCategory>();
            RecipeFoodItem = new HashSet<RecipeFoodItem>();
            UserFoodItem = new HashSet<UserFoodItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int FoodTypeId { get; set; }

        public virtual ICollection<FoodItemCategory> FoodItemCategory { get; set; }
        public virtual ICollection<RecipeFoodItem> RecipeFoodItem { get; set; }
        public virtual ICollection<UserFoodItem> UserFoodItem { get; set; }
        public virtual FoodType FoodType { get; set; }
    }
}
