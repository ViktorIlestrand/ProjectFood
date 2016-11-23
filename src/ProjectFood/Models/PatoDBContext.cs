using Microsoft.EntityFrameworkCore;
using ProjectFood.Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models.Entities
{
    public partial class PatoDBContext : DbContext
    {
        public PatoDBContext(DbContextOptions<PatoDBContext> options) : base(options)
        {

            
        }

    }
}
