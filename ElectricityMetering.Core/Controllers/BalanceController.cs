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
    public class BalanceController : Controller
    {
        public decimal Debt { get; set; }
        public decimal Advance { get; set; }
        public decimal Balance { get; set; }

        public void Calculate()
        {
            foreach (Owner owner in _context.Owners.ToList())
            {
                Add(owner.Balance);
            }
        }

        public void Add(decimal balance)
        {
            if (balance < 0)
            {
                Debt += balance;
            }
            else
            {
                Advance += balance;
            }

            Balance = Debt + Advance;
        }

        public void Reset()
        {
            Debt = 0;
            Advance = 0;
            Balance = 0;
        }
    }
}
