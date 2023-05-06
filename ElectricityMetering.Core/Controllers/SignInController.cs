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
            Role? role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            return role != null && role.Password == password;
        }

        public async Task<bool> RoleIsActiveAsync(string roleName)
        {
            Role? role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            return role != null && role.IsActive;
        }

        public async Task SignInAsync(string roleName)
        {
            Role? role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            if (role != null)
            {
                role.IsActive = true;
                await _context.SaveChangesAsync();
            }

            return;
        }

        public async Task ExitAsync(string roleName)
        {
            Role? role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            if (role != null)
            {
                role.IsActive = false;
                await _context.SaveChangesAsync();
            }

            return;
        }
    }
}
