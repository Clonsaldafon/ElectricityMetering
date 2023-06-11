using ElectricityMetering.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectricityMetering.Core.Controllers
{
    public class SignInController
    {
        private readonly ApplicationContext _context = new ApplicationContext();

        /// <summary>
        /// Checks whether the entered password is correct.
        /// </summary>
        /// <param name="roleName">Current role name.</param>
        /// <param name="password">Entered password.</param>
        /// <returns>Is it or not.</returns>
        public async Task<bool> PasswordIsCorrectAsync(string roleName, string password)
        {
            Role? role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            return role != null && role.Password == password;
        }
    }
}
