using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class KitchenStorage
    {
        public KitchenStorage()
        {
            User = new HashSet<User>();
            UserFoodItem = new HashSet<UserFoodItem>();
        }

        public int Id { get; set; }

        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<UserFoodItem> UserFoodItem { get; set; }
    }
}
