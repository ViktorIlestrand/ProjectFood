using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectFood.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
        public IActionResult Index()
        {
            return View();
        }
    }
}
