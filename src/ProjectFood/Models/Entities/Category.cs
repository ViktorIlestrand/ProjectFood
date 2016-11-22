using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class Category
    {
        public Category()
        {
            RecipeCategory = new HashSet<RecipeCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RecipeCategory> RecipeCategory { get; set; }
    }
}
