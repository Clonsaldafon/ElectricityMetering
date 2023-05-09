using ElectricityMetering.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controllers
{
    public class TariffController : Controller
    {
        private readonly ApplicationContext _context = new ApplicationContext();

        public List<Tariff> Tariffs { get; set; } = new List<Tariff>();

        public TariffController()
        {
            UpdateTariffs();
        }

        public bool NeedToUpdateTariff()
        {
            Tariff lastTariff = _context.Tariffs.OrderByDescending(t => t.Date).ToArray()[0];

            int day = DateTime.Today.Day;
            int month = DateTime.Today.Month;

            // test
            day = 1;
            month = 1;

            return day == 1 && (month == 1 || month == 7) && DateOnly.FromDateTime(DateTime.Today) > lastTariff.Date;
        }

        public async Task AddTariffAsync(string dateString, string priceString)
        {
            if (DateOnly.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
            {
                if (decimal.TryParse(priceString, out decimal price))
                {
                    await _repository.CreateTariffAsync(date, price);

                    UpdateTariffs();
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void UpdateTariffs()
        {
            Tariffs = _repository.GetTariffs();
        }
    }
}
