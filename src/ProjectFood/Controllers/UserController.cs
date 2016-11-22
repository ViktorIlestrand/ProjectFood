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

namespace ProjectFood.Controllers
{
    public class UserController : Controller
    {
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        IdentityDbContext identityContext;
        //PatoDBContext context;

        public UserController(IdentityDbContext identitycontext, UserManager<IdentityUser> usermanager, SignInManager<IdentityUser> signinmanager)
        {
            userManager = usermanager;
            signInManager = signinmanager;
            //this.context = context;
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

            else return View();
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
                return View(viewModel);

            #region createonce
            //// Skapa DB-schemat
            //await identityContext.Database.EnsureCreatedAsync();

            //// Create user
            //var user = new IdentityUser("Admin@admin.com");
            //var result = await userManager.CreateAsync(user, "Sommar2016!");
            #endregion

            var result = await signInManager.PasswordSignInAsync(
                viewModel.Email, viewModel.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(LoginVM.Email),
                    "Incorrect login credentials");
                return View(viewModel);
            }

            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction(nameof(UserController.Index));
            else
                return Redirect(returnUrl);
        }
    }
}
