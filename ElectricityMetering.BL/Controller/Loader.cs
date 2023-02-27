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
        public void LoadInfo(Garage garage, Owner owner, Payment payment, PricePerKw pricePerKw, string garageNumber)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (string.IsNullOrEmpty(garageNumber))
                {
                    garage = db.Garages.FirstOrDefault(g => g.Number == garageNumber);
                    if (garage is not null)
                    {
                        owner = db.Owners.FirstOrDefault(o => o.Garages.Contains(garage));
                        payment = db.Payments.FirstOrDefault(p => p.Owner == owner);
                        pricePerKw = db.PricesPerKw.Last();
                    }
                }
                else
                {
                    throw new ArgumentNullException("GarageNumber is null!");
                }
            }
        }
    }
}
