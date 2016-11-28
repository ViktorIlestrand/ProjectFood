using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectFood.Models.ViewModels.RecipeVM;
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
                .ThenInclude(o => o.FoodItem)
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

        public string SaveFoodItem(string food, int userId)
        {
            //kolla i databasen efter foodItem med samma namn
            var Id = FoodItem
                .Where(f => f.Name == food)
                .Select(f => f.Id);

            int foodItemId = Convert.ToInt32(Id.First());

            //checka ifall user redan har samma userFoodItem
            var itemAlreadyExists = UserFoodItem
                .Any(u => (u.FoodItemId == foodItemId) && (u.UserId == userId));

            string message = "Not added";

            if (!itemAlreadyExists)
            {
                //skapa ny userfooditem
                UserFoodItem newUserFoodItem = new Entities.UserFoodItem() { FoodItemId = foodItemId, UserId = userId, /*Expires = new DateTime(19, 01, 01)*/ };

                //spara i databasen
                UserFoodItem.Add(newUserFoodItem);
                SaveChanges();
                message = "Added";
            }
            return message;
        }

        public void changeDate(User user, string foodName, string date)
        {
            //existerar inte eftersom vi inte lyckats includa FoodItem när vi laddade UserFoodItem.
            //hint: vi laddade UserFoodItem i loulaUser
            bool exists = user.UserFoodItem
                .Any(u => u.FoodItem.Name == foodName);

            if (exists)
            {
                //istället för dummy data konvertera string date till vårt datetime
                DateTime dateTime = new DateTime(12, 12, 12);
                var userFoodItem = user.UserFoodItem
                    .Where(u => u.FoodItem.Name == foodName)
                    .Select(u => u.Expires = dateTime);
                SaveChanges();
            }
            //if !alreadyExists, skapa upp ny?
        }

        public List<RecipeVM> GetRecipesWithFoodItems(User user, List<FoodItem> foodItems)
        {
            var recipes = GetAllRecipes();

            //recipesMatched återspeglar en lista på recept som innehåller samtliga fooditems som är på väg att gå ut(1 eller flera)
            var recipesMatched = new List<Recipe>();
            
            foreach (var recipe in recipes)
            {
                var recipeingredients = GetFoodItemForRecipe(recipe);
                //matchedFoodItems håller reda på hur många fooditems som fått träff i receptet
                int matchedFoodItems = 0;

                foreach (var ingredient in recipeingredients)
                {
                    foreach (var fooditem in foodItems)
                    {
                        if (fooditem == ingredient.FoodItem)
                        {
                            //om vi får träff plusar vi på matchedfootitems
                            matchedFoodItems++;
                            break;
                        }
                    }
                }
                //Här checkar vi om vi har lika många träffar som fooditems. I så fall har vi ett matchande recept och
                //lägger till det i listan över recept som innehåller fooditemsen vi har skickat in
                if(matchedFoodItems == foodItems.Count)
                {
                    recipesMatched.Add(recipe);
                }
                
            }

            //det är här jag börjar reflektera över hur effektiv vår samt min kod egentligen är, men jag kör på ändå.
            //... så vi plockar ut en vanlig matchlista med hjälp av user
            var recipeVMsToReturn = GetMatchingRecipes(user);

            //sen börjar vi undersöka om VM-receptet finns med i matchedrecipes
            foreach (var item in recipeVMsToReturn)
            {
                bool keepRecipeInList = false;

                foreach (var recipe in recipesMatched)
                {
                    //om receptens namn stämmer med varandra behåller vi receptet i listan av VMs som kommmer returneras
                    if(recipe.Title == item.Title)
                    {
                        keepRecipeInList = true;
                        break;
                    }
                }
                //annars slänger vi ut den VMen.
                if (!keepRecipeInList)
                {
                    recipeVMsToReturn.Remove(item);
                }
            }
            //detta gjorde jag för att kunna returnera VMs, så vi ändå kan sortera listan efter hur väl det matchar
            //med userns kylskåp MEN med restriktionen att det bara skickas recept som innehåller matvarorna som
            // är på väg att gå ut :)
            return recipeVMsToReturn.OrderByDescending(s => s.MatchPercentage).ToList(); ;
            //gonatt!
        }

        public List<RecipeVM> GetMatchingRecipes(User user)
        {
            var recipevmsToReturn = new List<RecipeVM>();
            var recipes = GetAllRecipes(); 
            var useritems = new List<FoodItem>();

            foreach (var item in user.UserFoodItem)
            {
                useritems.Add(item.FoodItem);
            }
            
            foreach (var recipe in recipes)
            {
                var recipeingredients = GetFoodItemForRecipe(recipe);
                var listOfFoodMatches = new List<FoodMatching>();

                foreach (var ingredient in recipeingredients)
                {
                    bool match = false;

                    foreach (var useritem in useritems)
                    {
                        if(ingredient.FoodItem.Name == useritem.Name)
                        {
                            match = true;
                            break;
                        }
                    }

                    var foodmatch = new FoodMatching(match, ingredient.FoodItem.Name, recipe.Id);
                    listOfFoodMatches.Add(foodmatch);
                }

                double matchpercentage = GetMatchPercentage(listOfFoodMatches);
                var recipevm = new RecipeVM(recipe.Title, recipe.Portions, recipeingredients, listOfFoodMatches, matchpercentage, recipe.Instructions, recipe.ImageUrl, recipe.CookingTime);

                recipevmsToReturn.Add(recipevm);
            }
            var sortedList = recipevmsToReturn.OrderByDescending(s => s.MatchPercentage).ToList();

            return sortedList;

            
        }


        public double GetMatchPercentage(List<FoodMatching> list)
        {
            double truesey = 0;
            double falsey = 0;

            foreach (var item in list)
            {
                if (item.UserHasItem)
                {
                    truesey++;
                }else
                {
                    falsey++;
                }
            }
            //if (list.Count == 0)
            //    return 0;
            //else
            double result = (truesey / (truesey + falsey));
            return result;

        }

        public List<Recipe> GetAllRecipes()
        {
             var result = Recipe
                .Include(o => o.RecipeFoodItem)
                .ToList();            


            return result;
        }

        private List<RecipeFoodItem> GetFoodItemForRecipe(Recipe recipe)
        {
            var result = RecipeFoodItem
                .Include(p => p.FoodItem)
                .Where(p => recipe.Id == p.RecipeId)
                .ToList();

            return result;
        }

        
    }
}
