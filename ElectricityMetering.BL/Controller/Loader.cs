using ElectricityMetering.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.BL.Controller
{
    public class Loader
    {
        public bool CanLoadInfo(string garageNumber)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return !string.IsNullOrEmpty(garageNumber) && db.Garages.FirstOrDefault(g => g.Number == garageNumber) is not null;
            }
        }

        public void LoadInfo(Garage garage, Owner owner, Payment payment, PricePerKw pricePerKw, string garageNumber)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                garage = db.Garages.FirstOrDefault(g => g.Number == garageNumber);
                owner = db.Owners.FirstOrDefault(o => o.Garages.Contains(garage));
                payment = db.Payments.FirstOrDefault(p => p.Owner == owner);
                pricePerKw = db.PricesPerKw.Last();
            }
        }
    }
}
