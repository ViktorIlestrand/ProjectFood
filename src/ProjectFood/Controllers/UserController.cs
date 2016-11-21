using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectFood.Models.Entities;


namespace ProjectFood.Controllers
{
    public class UserController : Controller
    {
        PatoDBContext context;

        public UserController(PatoDBContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
