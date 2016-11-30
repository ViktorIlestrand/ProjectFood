using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectFood.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ProjectFood.Models.ViewModels;
using ProjectFood.Models.ViewModels.UserVM;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ProjectFood.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        IdentityDbContext identityContext;
        PatoDBContext context;
        private readonly ILogger _logger;

        public UserController(PatoDBContext context,
            IdentityDbContext identitycontext,
            UserManager<IdentityUser> usermanager,
            SignInManager<IdentityUser> signinmanager,
            ILoggerFactory loggerFactory)
        {
            userManager = usermanager;
            signInManager = signinmanager;
            this.context = context;
            identityContext = identitycontext;
            _logger = loggerFactory.CreateLogger<UserController>();
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var user = new IdentityUser(viewModel.UserName);
            var result = await userManager.CreateAsync(user, viewModel.Password);

            if (!result.Succeeded)
            {
                string er = "";
                foreach (var item in result.Errors)
                {
                    er += item.Description.ToString();
                }
                ModelState.AddModelError(nameof(RegisterVM.UserName), er);
                return View(viewModel);
            }
            await userManager.SetEmailAsync(user, viewModel.Email);

            var entityUser = new User();
            entityUser.AspNetId = user.Id;
            context.User.Add(entityUser);
            context.SaveChanges();

            var result2 = await signInManager.PasswordSignInAsync(
                viewModel.UserName, viewModel.Password, false, false);

            var boolen = signInManager.IsSignedIn(User);

            return RedirectToAction(nameof(MyKitchen));
        }

        public async Task<IActionResult> MyKitchen()
        {
            //Här hämtar vi ut Loula.Users alla proppar och lagrar i en Userinstans som vi kallar loulaUser
            var loulaUser = await context.GetLoulaUser(User);
            var id = loulaUser.Id;
            var myKitchenVM = new MyKitchenVM(loulaUser.UserFoodItem.ToList(), context.GetPopularFoodItems(10));

            return View(myKitchenVM);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM viewModel, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            #region createonce
            //// Skapa DB-schemat
            //await identityContext.Database.EnsureCreatedAsync();

            //// Create user
            //var user = new IdentityUser("Admin@admin.com");
            //var result = await userManager.CreateAsync(user, "Sommar2016!");
            #endregion

            //Detta skapar upp en icke-persistent cookie som lagrar username
            var result = await signInManager.PasswordSignInAsync(
                viewModel.UserName, viewModel.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(LoginVM.UserName),
                    "Felaktiga inloggningsuppgifter");
                return View(viewModel);
            }

            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction(nameof(MyKitchen));
            else
                return RedirectToAction(nameof(MyKitchen));
        }

        // POST: /Account/LogOff
        //[HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {

            await signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }



        [HttpGet]
        public string FoodQuery([FromQuery]string query)
        {
            var list = context.GetAllFoodItems(query);

            return JsonConvert.SerializeObject(list);
        }

        [HttpPost]
        public async Task<string> SaveFood([FromForm]string food)
        {
            var loulaUser = await context.GetLoulaUser(User);
            
            var status = context.SaveFoodItem(food, loulaUser.Id);

            return JsonConvert.SerializeObject(status);
        }

        [HttpPost]
        public async Task<string> RemoveFood([FromForm]string foodName)
        {
            var loulaUser = await context.GetLoulaUser(User);

            var status = context.RemoveFoodFromKitchen(foodName, loulaUser);

            return JsonConvert.SerializeObject(status);
        }

        [HttpGet]
        public string MyFood()
        {
            var loulaUser = context.GetLoulaUser(User);

            var userFood = loulaUser.Result.UserFoodItem;
            
            return JsonConvert.SerializeObject(userFood);
        }

        public async Task<IActionResult> Recipes()
        {
            
            var loulaUser = await context.GetLoulaUser(User);

            var expiringFoodItems = context.CheckExpiringUserFoodItems(loulaUser);

            if (expiringFoodItems.Count != 0)
            {
                return View(context.GetRecipesWithFoodItems(loulaUser, expiringFoodItems));
            }else
            {
                return View(context.GetMatchingRecipes(loulaUser));
            }
        }

        [HttpPost]
        public async Task<string> SaveExpireDate([FromForm]string foodName, [FromForm] string date)
        {
            var loulaUser = await context.GetLoulaUser(User);

            //plocka fram userfooditem utifrån foodname och user
            context.changeDate(loulaUser, foodName, date);

            return JsonConvert.SerializeObject("ok");
        }

        public IActionResult RecipeDetails(int id)
        {
            return View(context.GetRecipeById(id));
        } 


    }
}
