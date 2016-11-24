using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectFood.Models.ViewModels.UserVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models.Entities
{
    public partial class PatoDBContext : DbContext
    {
        UserManager<IdentityUser> userManager;
        public PatoDBContext(DbContextOptions<PatoDBContext> options, UserManager<IdentityUser> userManager) : base(options)
        {
            this.userManager = userManager;

        }

        public async Task<List<UserFoodItem>> GetAllUserFoodItems() //här måste en session skickas in 
            //för att kitchen ska veta vilken användares matvaror som ska visas upp.
        {
            
            return null;
        }

        //Inparametern kommer in med hjälp av den icke-persistenta cookien (User.Identity.Name)
        public async Task<User> GetLoulaUser(string username)
        {
            //UserManagern kan med hjälp av användarnamnet hämta ut en IdentityUser från databasen enl nedan
            var identityUser = await userManager.FindByNameAsync(username);
            //Här hämtar vi ut Loula.User.Id med hjälp av vår IdentityUser 
            var loulaUser = this.User.SingleOrDefault(
                o => o.AspNetId == identityUser.Id
                );

            return loulaUser;
        }

        public List<FoodItem> GetUserFoodItems(int userId)
        {
            var kitchenStorage = KitchenStorage.SingleOrDefault(
                k => k.UserId == userId);

            var userFood = UserFoodItem.Where(
                f => f.KitchenStorageId == kitchenStorage.Id)
                .Select(f => f.FoodItem
                );

            var foodList = userFood.ToList();
            return foodList;
        }


    }
}
