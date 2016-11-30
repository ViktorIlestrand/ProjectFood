using ProjectFood.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectFood.Models.ViewModels.UserVM
{
    public class UserFoodItemVM
    {
        public DateTime? Expires { get; set; }

        public virtual FoodItem FoodItem { get; set; }

        private bool hasExpired;
        public bool HasExpired { get { return CheckHasExpired(); } set { hasExpired = value; } }


        //public UserFoodItemVM()
        //{
        //    HasExpired = CheckHasExpired();
        //}

        private bool CheckHasExpired()
        {
            bool hasExpired = false;
            DateTime thisDate = DateTime.Now;
            try
            {
                if (Expires != null)
                {
                    DateTime expireDate = Convert.ToDateTime(Expires);
                    int result = DateTime.Compare(thisDate, expireDate);
                    if (result > 0)
                    {
                        hasExpired = true;
                    }
                }
            }
            catch
            {
                //all is fine
            }

            return hasExpired;
        }

    }
}
