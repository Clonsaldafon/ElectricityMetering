using ElectricityMetering.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controllers
{
    public class SignInController
    {
        private readonly ApplicationContext _context = new ApplicationContext();

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
