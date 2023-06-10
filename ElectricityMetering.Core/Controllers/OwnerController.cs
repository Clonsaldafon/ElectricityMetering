using ElectricityMetering.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controllers
{
    public class OwnerController
    {
        private CounterController _counterController = new CounterController();
        private TariffController _tariffController = new TariffController();

        /// <summary>
        /// Updates the owner's balance based on payments.
        /// </summary>
        /// <param name="owner">Current owner.</param>
        /// <returns></returns>
        public async Task UpdateBalanceAsync(Owner owner)
        {
            decimal balance = 0;
            foreach (Payment payment in owner.Payments)
            {
                balance += payment.Cash + payment.NoneCash;
            }

            await Repository.SaveOwnerAsync(owner, balance);
        }

        /// <summary>
        /// Updates the owner's balance based on consumption.
        /// </summary>
        /// <param name="owner">Current owner.</param>
        /// <param name="counter">Current counter.</param>
        /// <returns></returns>
        public async Task UpdateBalanceAsync(Owner owner, Counter counter)
        {
            _counterController.CalculateConsumption(counter);

            int[] consumptionNow = _counterController.ConsumptionNow;
            int[] consumptionOneYearAgo = _counterController.ConsumptionOneYearAgo;
            int[] consumptionTwoYearsAgo = _counterController.ConsumptionTwoYearsAgo;

            if (_tariffController.Tariffs.Count >= 6)
            {
                decimal balance = 0;
                int startMonth = 1;
                int endMonth = 6;

                for (int i = 0; i < 2; i++)
                {
                    balance -= CalculateBalanceByConsumption(_tariffController.Tariffs[i], consumptionNow, startMonth, endMonth);
                    startMonth += 6;
                    endMonth += 6;
                }

                startMonth = 1;
                endMonth = 6;

                for (int i = 2; i < 4; i++)
                {
                    balance -= CalculateBalanceByConsumption(_tariffController.Tariffs[i], consumptionOneYearAgo, startMonth, endMonth);
                    startMonth += 6;
                    endMonth += 6;
                }

                startMonth = 1;
                endMonth = 6;

                for (int i = 4; i < 6; i++)
                {
                    balance -= CalculateBalanceByConsumption(_tariffController.Tariffs[i], consumptionTwoYearsAgo, startMonth, endMonth);
                    startMonth += 6;
                    endMonth += 6;
                }

                await Repository.SaveOwnerAsync(owner, balance);
            }

            await UpdateBalanceAsync(owner);
        }

        private decimal CalculateBalanceByConsumption(Tariff tariff, int[] consumption, int startMonth, int endMonth)
        {
            decimal balance = 0;

            for (int i = startMonth - 1; i < endMonth; i++)
            {
                balance += consumption[i] * tariff.Price;
            }

            return balance;
        }
    }
}
