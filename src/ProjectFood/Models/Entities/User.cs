using System;
using System.Collections.Generic;

namespace ProjectFood.Models.Entities
{
    public partial class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
