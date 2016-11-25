using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class Recipe
    {
        public Recipe()
        {
            RecipeFoodItem = new HashSet<RecipeFoodItem>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public int Portions { get; set; }
        public int CookingTime { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<RecipeFoodItem> RecipeFoodItem { get; set; }
    }
}
