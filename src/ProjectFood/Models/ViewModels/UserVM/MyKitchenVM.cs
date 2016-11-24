using ProjectFood.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models.ViewModels.UserVM
{
    public class MyKitchenVM //ska visa upp en Lista med matvaror användaren har i sitt kylskåp. ska även
                            //visa upp en lista med populära matvaror att lägga i kylsåket, och en sökruta för 
                            //matvaror med förslag. 

    {
        public List<UserFoodItem> MyFood { get; set; }

        public List<FoodItem> AddableFood { get; set; }


        public MyKitchenVM(List<UserFoodItem> userFoodList)
        {
            MyFood = userFoodList;
            //AddableFood = GetPopularFood(10);
        }
    }
}
