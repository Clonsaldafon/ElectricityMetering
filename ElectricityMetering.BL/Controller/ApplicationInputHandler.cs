﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.BL.Controller
{
    public static class ApplicationInputHandler
    {
        public static bool PasswordIsCorrect(string roleName, string password)
        {
            using(ApplicationContext db = new ApplicationContext())
            {
                var role = db.Roles.ToList().FirstOrDefault(item => item.Name == roleName);

                return !string.IsNullOrEmpty(role?.Name) && role.Password == password;
            }
        }
    }
}