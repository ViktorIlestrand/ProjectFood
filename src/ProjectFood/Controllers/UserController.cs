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

namespace ProjectFood.Controllers
{
    public class UserController : Controller
    {
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        IdentityDbContext identityContext;
        PatoDBContext context;

        public UserController(PatoDBContext context, UserManager<IdentityUser> usermanager, SignInManager<IdentityUser> signinmanager, IdentityDbContext identitycontext)
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM viewModel, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            // Skapa DB-schemat
            await identityContext.Database.EnsureCreatedAsync();

            // Create user
            var user = new IdentityUser("Admin@admin.com");
            var result = await userManager.CreateAsync(user, "Sommar2016!");

            //var result = await signInManager.PasswordSignInAsync(
            //    viewModel.Email, viewModel.Password, false, false);

            //if (!result.Succeeded)
            //{
            //    ModelState.AddModelError(nameof(UserLoginVM.Email),
            //        "Incorrect login credentials");
            //    return View(viewModel);
            //}

            //if (string.IsNullOrWhiteSpace(returnUrl))
            //    return RedirectToAction(nameof(UserController.Index));
            //else
            //    return Redirect(returnUrl);

            return Redirect(returnUrl);
        }
    }
}
