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


        //Inparametern kommer in med hjälp av den icke-persistenta cookien (User.Identity.Name)
        public async Task<User> GetLoulaUser(string username)
        {
            //UserManagern kan med hjälp av användarnamnet hämta ut en IdentityUser från databasen enl nedan
            var identityUser = await userManager.FindByNameAsync(username);
            //Här hämtar vi ut Loula.User.Id med hjälp av vår IdentityUser 
            var loulaUser = this.User
                .Include(o => o.UserFoodItem)
                .SingleOrDefault(
                o => o.AspNetId == identityUser.Id
                );

            return loulaUser;
        }

        //public List<UserFoodItem> GetUserFoodItems(User user)
        //{
        //    return user.UserFoodItem.ToList();
        //}

        public void AddFoodToKitchen(User user, FoodItem food, DateTime expiryDate)
        {
            try
            {
                var userFoodItem = new UserFoodItem();
                userFoodItem.Expires = expiryDate;
                userFoodItem.FoodItem = food;

                user.UserFoodItem.Add(userFoodItem);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.ToString();
                System.IO.File.WriteAllText(@"C:\Users\Administrator\Documents\Visual Studio 2015\Projects\ProjectFood\src\ProjectFood\ErrorLog.txt", errorMsg);
            }

        }
        public void RemoveFoodFromKitchen(User user, UserFoodItem food)
        {
            user.UserFoodItem.Remove(food);
        }

        public List<FoodItem> GetPopularFoodItems(int number)
        {
            var list = new List<FoodItem>();

            Random rand = new Random();

            for (int i = 0; i < number; i++)
            {
                var foodItems = FoodItem.ToList();
                var item = foodItems.ElementAt(rand.Next(2, 75));
                list.Add(item);
            }

            return list;
        }

        public List<string> GetAllFoodItems(string query)
        {
            var list = this.FoodItem
                .Select(p => p.Name)
                .Where(p => p.Contains(query))
                .ToList();

            return list;
        }

        public void SaveFoodItem(string food, int userId)
        {
            //kolla i databasen efter foodItem med samma namn
            var Id = FoodItem
                .Where(f => f.Name == food)
                .Select(f => f.Id);

            int foodItemId = Convert.ToInt32(Id.First());

            //checka ifall user redan har samma userFoodItem
            var itemAlreadyExists = UserFoodItem
                .Any(u => (u.FoodItemId == foodItemId) && (u.UserId == userId));

            if (!itemAlreadyExists)
            {
                //skapa ny userfooditem
                UserFoodItem newUserFoodItem = new Entities.UserFoodItem() { FoodItemId = foodItemId, UserId = userId };

                //spara i databasen
                UserFoodItem.Add(newUserFoodItem);
                SaveChanges();
            }
        }
    }
}
