using ElectricityMetering.Core.Models;

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
