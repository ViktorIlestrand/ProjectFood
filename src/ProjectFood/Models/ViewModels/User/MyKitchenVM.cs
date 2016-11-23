using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models.ViewModels.User
{
    public class MyKitchenVM //ska visa upp en Lista med matvaror användaren har i sitt kylskåp. ska även
                            //visa upp en lista med populära matvaror att lägga i kylsåket, och en sökruta för 
                            //matvaror med förslag. 
    {
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }

    }
}
