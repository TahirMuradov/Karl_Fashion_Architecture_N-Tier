using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
   public static class UserNameHelper
    {
       public static string CreatingUserName(string FirstName, string LastName)
        {
            string userName = LastName + FirstName;

            for (int i = 0; i < 6; i++)
            {
                Random random = new Random();
                userName += random.Next(10);

            }
            return userName;
        }
    }
}
