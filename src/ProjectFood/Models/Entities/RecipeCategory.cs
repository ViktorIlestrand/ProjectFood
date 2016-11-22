using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class RecipeCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int RecipeId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
