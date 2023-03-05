using ElectricityMetering.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controller
{
    public class Repository
    {
        public bool CanCreateNewGarage(string garageNumber)
        {
            using(ApplicationContext db = new ApplicationContext())
            {
                return db.Garages.FirstOrDefault(g => string.Equals(g.Number, garageNumber)) is null;
            }
        }

        public void CreateNewGarage(string garageNumber)
        {
            using(ApplicationContext db = new ApplicationContext())
            {
                string sealNumber = "-";
                string counterNumber = "-";
                DateOnly sealDate = new DateOnly();
                Owner owner = new Owner { Name = "-", Balance = 0 };
                //int[] indications = new int[36];

                db.Owners.Add(owner);
                db.SaveChanges();

                Garage garage = new Garage
                {
                    Number = garageNumber,
                    SealNumber = sealNumber,
                    CounterNumber = counterNumber,
                    SealDate = sealDate,
                    Owner = owner,
                    //Indications = indications
                };
  
                db.Garages.Add(garage);
                db.SaveChanges();
            }
        }

        public bool CanLoadInfo(string garageNumber)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Garages.FirstOrDefault(g => string.Equals(g.Number, garageNumber)) is not null;
            }
        }

        public Garage LoadInfoByGarageNumber(string garageNumber)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Garages.First(g => string.Equals(g.Number, garageNumber));
            }
        }

        /*public Garage LoadInfo(string garageNumber)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Garages.FirstOrDefault(g => string.Equals(g.Number, garageNumber));
            }
        }*/

        /*public Owner LoadInfo(Garage garage)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Owner owner = db.Owners.FirstOrDefault(o => o.Garages.Contains(garage));

                if (owner.Garages is null)
                {
                    owner.Garages = new List<Garage> { garage };
                }

                return owner;
            }
        }*/

        /*public Payment LoadInfo(Owner owner)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Payment payment = db.Payments.OrderBy(p => p.Id).LastOrDefault(p => p.Owner is owner);

                if (payment is null)
                {
                    payment = new Payment { Date = new DateOnly(), Cash = 0, NonCash = 0, Total = 0 };
                }

                return payment;
            }
        }*/

        /*public Tariff LoadInfo()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.PricesPerKw.OrderBy(p => p.Id).LastOrDefault();
            }
        }*/
    }
}
