using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class KitchenStorage
    {
        public int Id { get; set; }
        public int UserIngredientId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual UserIngredient UserIngredient { get; set; }
    }
}
