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

        public async Task AddUserAsync(RegisterVM viewModel)
        {
            User user = new User
            {
                Email = viewModel.Email,
                Name = viewModel.UserName

            };

            await this.SaveChangesAsync();
            User.Add(user);

        }

    }
}
