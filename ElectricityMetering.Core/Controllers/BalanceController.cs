using ElectricityMetering.Core.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ElectricityMetering.Core.Controllers
{
    public class BalanceController
    {
        private ApplicationContext _context = new ApplicationContext();

        public decimal Debt { get; set; }
        public decimal Advance { get; set; }
        public decimal Balance { get; set; }

        public void Calculate()
        {
            foreach (Owner owner in _context.Owners.ToList())
            {
                if (owner.Balance < 0)
                {
                    Debt += owner.Balance;
                }
                else
                {
                    Advance += owner.Balance;
                }

                Balance = Debt + Advance;
            }
        }
    }
}
