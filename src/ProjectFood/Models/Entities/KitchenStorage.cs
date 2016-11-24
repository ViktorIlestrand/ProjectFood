using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class KitchenStorage
    {
        public KitchenStorage()
        {
            UserFoodItem = new HashSet<UserFoodItem>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<UserFoodItem> UserFoodItem { get; set; }
        public virtual User User { get; set; }
    }
}
