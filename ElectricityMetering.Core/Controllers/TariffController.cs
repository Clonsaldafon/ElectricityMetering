using ElectricityMetering.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controllers
{
    public class TariffController
    {
        public List<Tariff> Tariffs { get; set; } = new List<Tariff>();

        public TariffController()
        {
            UpdateTariffs();
        }

        /// <summary>
        /// Checks whether the tariff needs to be updated.
        /// </summary>
        /// <returns>Is it necessary or not.</returns>
        public bool NeedToUpdateTariff()
        {
            Tariff lastTariff = Repository.GetTariffs()[0];

            int day = DateTime.Today.Day;
            int month = DateTime.Today.Month;

            return day == 1 && (month == 1 || month == 7) && DateOnly.FromDateTime(DateTime.Today) > lastTariff.Date;
        }

        /// <summary>
        /// Add a new tariff to the database.
        /// </summary>
        /// <param name="dateString">The date from which the tariff begins to operate.</param>
        /// <param name="priceString">Price per kw.</param>
        /// <returns></returns>
        public async Task AddTariffAsync(string dateString, string priceString)
        {
            if (DateOnly.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
            {
                if (decimal.TryParse(priceString, out decimal price))
                {
                    await Repository.CreateTariffAsync(date, price);

                    UpdateTariffs();
                }
            }
        }

        private void UpdateTariffs()
        {
            Tariffs = Repository.GetTariffs();
        }
    }
}
