﻿using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class User
    {
        public User()
        {
            KitchenStorage = new HashSet<KitchenStorage>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public virtual ICollection<KitchenStorage> KitchenStorage { get; set; }
    }
}
