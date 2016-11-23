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
using ProjectFood.Models.ViewModels.User;
using Microsoft.Extensions.Logging;

namespace ProjectFood.Controllers
{
    public class UserController : Controller
    {
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        IdentityDbContext identityContext;
        PatoDBContext context;
        private readonly ILogger _logger;

        public UserController(PatoDBContext context, IdentityDbContext identitycontext, UserManager<IdentityUser> usermanager, SignInManager<IdentityUser> signinmanager)
        {
            userManager = usermanager;
            signInManager = signinmanager;
            this.context = context;
            identityContext = identitycontext;

        }

        public string Index()
        {
            return $"You are logged in as {User.Identity.Name}";
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

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
                        
            return RedirectToAction(nameof(MyKitchen));
        }

        public IActionResult MyKitchen()
        {
            return View();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(UserController.Index), "Home");
        }
    }
}
