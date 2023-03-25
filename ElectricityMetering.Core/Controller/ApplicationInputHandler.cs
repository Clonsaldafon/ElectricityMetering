using ElectricityMetering.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controller
{
    public class ApplicationInputHandler
    {
        public async Task<bool> PasswordIsCorrectAsync(string roleName, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Role? role = await db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

                return role != null && role.Password == password;
            }
        }
    }
}
