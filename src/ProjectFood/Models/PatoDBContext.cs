using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectFood.Models.ViewModels.AdminVM;
using ProjectFood.Models.ViewModels.RecipeVM;
using ProjectFood.Models.ViewModels.UserVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public async Task<User> GetLoulaUser(ClaimsPrincipal user)
        {
            //UserManagern kan med hjälp av användarnamnet hämta ut en IdentityUser från databasen enl nedan
            //Här hämtar vi ut Loula.User.Id med hjälp av vår IdentityUser 
            var loulaUser = await this.User
                .Include(o => o.UserFoodItem)
                .ThenInclude(o => o.FoodItem)
                .SingleOrDefaultAsync(
                o => o.AspNetId == userManager.GetUserId(user)
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
        public string RemoveFoodFromKitchen(string foodName, User user)
        {
            var foodToRemove = UserFoodItem
                .FirstOrDefault(u => (u.FoodItem.Name == foodName) && (u.UserId == user.Id));
            UserFoodItem.Remove(foodToRemove);
            SaveChanges();
            return "ok";
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
                var dateTime = Convert.ToDateTime(date);
                var userFoodItem = user.UserFoodItem
                    .Where(u => u.FoodItem.Name == foodName)
                    .Select(u => u.Expires = dateTime).ToArray();
                SaveChanges();
            }
            //if !alreadyExists, skapa upp ny?
        }

        public List<RecipeVM> GetRecipesWithFoodItems(User user, List<UserFoodItem> userFoodItems)
        {

            var recipes = GetAllRecipes();
            //recipesMatched återspeglar en lista på recept som innehåller samtliga fooditems som är på väg att gå ut(1 eller flera)
            var recipesMatched = new List<Recipe>();

            bool resultIsEmpty = true;
            int numberOfUserfoodItems = userFoodItems.Count;

            while (resultIsEmpty)
            {

                foreach (var recipe in recipes)
                {
                    var recipeingredients = GetFoodItemForRecipe(recipe);
                    //matchedFoodItems håller reda på hur många fooditems som fått träff i receptet
                    int matchedFoodItems = 0;

                    foreach (var ingredient in recipeingredients)
                    {
                        var foodItems = new List<FoodItem>();

                        foreach (var item in userFoodItems)
                        {
                            foodItems.Add(item.FoodItem);
                        }

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
                    if (matchedFoodItems == numberOfUserfoodItems)
                    {
                        recipesMatched.Add(recipe);
                    }
                }
                if (recipesMatched.Count == 0)
                {
                    numberOfUserfoodItems--;
                }
                else
                {
                    resultIsEmpty = false;
                }
            }

            //det är här jag börjar reflektera över hur effektiv vår samt min kod egentligen är, men jag kör på ändå.
            //... så vi plockar ut en vanlig matchlista med hjälp av user
            var recipeVMsToReturn = GetMatchingRecipes(user);
            var indexesToRemove = new List<int>();


            for (int i = 0; i < recipeVMsToReturn.Count; i++)
            {
                bool keepRecipeInList = false;
                foreach (var recipe in recipesMatched)
                {
                    //om receptens namn stämmer med varandra behåller vi receptet i listan av VMs som kommmer returneras
                    if (recipe.Title == recipeVMsToReturn[i].Title)
                    {
                        keepRecipeInList = true;
                        break;
                    }
                }
                //annars slänger vi ut den VMen.
                if (!keepRecipeInList)
                {
                    indexesToRemove.Add(i);
                }

            }

            indexesToRemove.Reverse();

            foreach (var index in indexesToRemove)
            {
                recipeVMsToReturn.RemoveAt(index);
            }

            return recipeVMsToReturn.OrderByDescending(s => s.MatchPercentage).ToList();
            //sen börjar vi undersöka om VM-receptet finns med i matchedrecipes

            //foreach (var item in tmplist)
            //{
            //    bool keepRecipeInList = false;

            //    foreach (var recipe in recipesMatched)
            //    {
            //        //om receptens namn stämmer med varandra behåller vi receptet i listan av VMs som kommmer returneras
            //        if (recipe.Title == item.Title)
            //        {
            //            keepRecipeInList = true;
            //            break;
            //        }
            //    }
            //    //annars slänger vi ut den VMen.
            //    if (!keepRecipeInList)
            //    {
            //        recipeVMsToReturn.Remove(item);
            //    }
            //}
            //detta gjorde jag för att kunna returnera VMs, så vi ändå kan sortera listan efter hur väl det matchar
            //med userns kylskåp MEN med restriktionen att det bara skickas recept som innehåller matvarorna som
            // är på väg att gå ut :)

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
                        if (ingredient.FoodItem.Name == useritem.Name)
                        {
                            match = true;
                            break;
                        }
                    }

                    var foodmatch = new FoodMatching(match, ingredient.FoodItem.Name, recipe.Id);
                    listOfFoodMatches.Add(foodmatch);
                }

                double matchpercentage = GetMatchPercentage(listOfFoodMatches);
                var recipevm = new RecipeVM(recipe.Id, recipe.Title, recipe.Portions, recipeingredients, listOfFoodMatches, matchpercentage, recipe.Instructions, recipe.ImageUrl, recipe.CookingTime);

                recipevmsToReturn.Add(recipevm);
            }

            var sortedList = recipevmsToReturn.OrderByDescending(p => p.MatchPercentage).ToList();
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
                }
                else
                {
                    falsey++;
                }
            }
            if (list.Count == 0)
            {
                return 0;
            }
            else
            {
                double result = (truesey / (truesey + falsey));
                return result;
            }
        }

        public List<Recipe> GetAllRecipes()
        {
            var result = Recipe
               .Include(o => o.RecipeFoodItem)
               .ThenInclude(o => o.FoodItem)
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

        private User GetExpiryDates (User user)
        {
            var result = User
                .Include(p => p.UserFoodItem)
                .ThenInclude(p => p.Expires)
                .SingleOrDefault(p => p.Id == user.Id);

            return result;
        }

        public List<UserFoodItem> CheckExpiringUserFoodItems(User user)
        {
            //skapa upp en tom lista att adda utgående matvaror till om det finns sådana
            var foodItemsExpiring = new List<UserFoodItem>();
            //lagrar dagens datum i en variabel och adderar 2 dagar till den, för att få med matvaror
            //som går ut inom 48 timmar
            DateTime dateTimeNow = DateTime.Now.AddDays(2);

            //loopar igenom användarens matvarors utgångsdatum
            foreach (var item in user.UserFoodItem)
            {
                var expiryDate = item.Expires.GetValueOrDefault();   

                //nullcheck för DateTime (null är 0001-01-01 00:00:00)
                if (expiryDate.Year > 1)
                {
                    var result = DateTime.Compare(dateTimeNow, expiryDate);
                    //om matvaran är på väg att gå ut så lägger vi till den i listan foodItemsExpiring
                    if (result >= 0)
                    foodItemsExpiring.Add(item);
                }
                else
                {

                }
            }

            return foodItemsExpiring;
        }

        public RecipeDetailsVM GetRecipeById(int id)
        {
            var result = Recipe
                .Include(p => p.RecipeFoodItem)
                .ThenInclude(p => p.FoodItem)
                .FirstOrDefault(p => p.Id == id);

            var recipeDetailsVM = new RecipeDetailsVM(result.Title, result.Instructions, result.Portions, result.CookingTime, result.ImageUrl, result.RecipeFoodItem.ToList());

            return recipeDetailsVM;
        }

        public async Task AddFoodItemToDB(AddFoodItemVM viewModel)
        {
            var newFoodItem = new FoodItem
            {
                Name = viewModel.Name,
                FoodTypeId = viewModel.FoodTypeId
            };

            FoodItem.Add(newFoodItem);

            await this.SaveChangesAsync();
            
        }
        public AddFoodItemVM GetAllFoodTypes()
        {
            var result = FoodType.ToList();
            var viewModel = new AddFoodItemVM("", 0, result);
            return viewModel;
        }

        public async Task AddRecipeToDB(AddRecipeVM viewModel)
        {
            var newRecipe = new Recipe
            {
                Title = viewModel.Title,
                Instructions = viewModel.Instructions,
                Portions = viewModel.Portions,
                CookingTime = viewModel.CookingTime,
                ImageUrl = viewModel.ImageUrl
            };

            Recipe.Add(newRecipe);

            await this.SaveChangesAsync();

        }

        public AllUserRecipesVM GetRecipeLists(User user)
        {
            var viewModel = new AllUserRecipesVM();

            var expiringFoodItems = CheckExpiringUserFoodItems(user);

            var result1 = GetRecipesWithFoodItems(user, expiringFoodItems);
            var result2 = GetMatchingRecipes(user);

            var indexesToRemove = new List<int>();

            for (int i = 0; i < result2.Count; i++)
            {
                foreach (var expiringRecipe in result1)
                {
                    if (result2[i].Title == expiringRecipe.Title )
                    {
                        indexesToRemove.Add(i);
                    }
                }
            }

            indexesToRemove.Reverse();

            foreach (var index in indexesToRemove)
            {
                result2.RemoveAt(index);
            }

            viewModel.expiringRecipes = result1;
            viewModel.matchedRecipes = result2;

            return viewModel;
        }
    }
}
