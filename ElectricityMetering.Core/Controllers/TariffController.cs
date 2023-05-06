using ElectricityMetering.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controllers
{
    public class TariffController
    {
        private readonly ApplicationContext _context = new ApplicationContext();

        public TariffController()
        {
            
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
    }
}
