using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class User
    {
        public int Id { get; set; }
        public string AspNetId { get; set; }
        public int KitchenStorageId { get; set; }

        public virtual KitchenStorage KitchenStorage { get; set; }
    }
}
