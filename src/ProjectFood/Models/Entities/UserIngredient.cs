using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class UserIngredient
    {
        public UserIngredient()
        {
            KitchenStorage = new HashSet<KitchenStorage>();
        }

        public int Id { get; set; }
        public int Ingredient { get; set; }
        public DateTime? Expires { get; set; }

        public virtual ICollection<KitchenStorage> KitchenStorage { get; set; }
        public virtual Ingredient IngredientNavigation { get; set; }
    }
}
