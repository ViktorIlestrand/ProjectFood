using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectFood.Models.ViewModels.AdminVM;
using ProjectFood.Models.Entities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectFood.Controllers
{
    public class AdminController : Controller
    {
        PatoDBContext context;
        public AdminController(PatoDBContext context)
        {
            this.context = context;
        }
        public IActionResult AddFoodItem()
        {
            return View(context.GetAllFoodTypes());
        }

        [HttpPost]
        public async Task<IActionResult> AddFoodItem(AddFoodItemVM addFood)
        {
            await context.AddFoodItemToDB(addFood);
            return View(context.GetAllFoodTypes());
        }
        [HttpGet]
        public IActionResult AddRecipe()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRecipe(AddRecipeVM viewModel)
        {
            await context.AddRecipeToDB(viewModel);
            return View();
        }
    }
}
