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
        public decimal Debt { get; set; }
        public decimal Advance { get; set; }
        public decimal Balance { get; set; }

        /// <summary>
        /// Calculates the balances of all owners.
        /// </summary>
        public void Calculate()
        {
            foreach (Owner owner in Repository.GetOwners())
            {
                Add(owner.Balance);
            }
        }

        private void Add(decimal balance)
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

        /// <summary>
        /// Resets the debt, advance, and balance values to 0.
        /// </summary>
        public void Reset()
        {
            Debt = 0;
            Advance = 0;
            Balance = 0;
        }
    }
}
