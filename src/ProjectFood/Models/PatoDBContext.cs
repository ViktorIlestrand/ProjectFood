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

        public async Task<List<UserFoodItem>> GetAllUserFoodItems() //här måste en session skickas in 
            //för att kitchen ska veta vilken användares matvaror som ska visas upp.
        {
            
            return null;
        }

    }
}
