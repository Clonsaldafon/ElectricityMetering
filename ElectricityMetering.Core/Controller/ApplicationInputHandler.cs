using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controller
{
    public static class ApplicationInputHandler
    {
        public static bool PasswordIsCorrect(string roleName, string password)
        {
            using(ApplicationContext db = new ApplicationContext())
            {
                var role = db.Roles.ToList().FirstOrDefault(item => string.Equals(item.Name, roleName));

                return !string.IsNullOrEmpty(role.Name) && string.Equals(role.Password, password);
            }
        }
    }
}
