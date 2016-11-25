using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class User
    {
        public User()
        {
            UserFoodItem = new HashSet<UserFoodItem>();
        }

        public int Id { get; set; }
        public string AspNetId { get; set; }

        public virtual ICollection<UserFoodItem> UserFoodItem { get; set; }
    }
}
