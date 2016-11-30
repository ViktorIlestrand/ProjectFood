using ProjectFood.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models.ViewModels.AdminVM
{
    public class AddFoodItemVM
    {
        public string Name { get; set; }
        public int FoodTypeId { get; set; }

        public List<FoodType> ListOfFoodTypes { get; set; }

        public AddFoodItemVM()
        {

        }
        public AddFoodItemVM(string name, int foodTypeId, List<FoodType> listOfFoodTypes)
        {
            Name = name;
            FoodTypeId = foodTypeId;
            ListOfFoodTypes = listOfFoodTypes;
        }
    }
}
