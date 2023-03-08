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

        public void CreateNewGarage(string garageNumber, string sealNumber = "-", string counterNumber = "-", DateOnly sealDate = new DateOnly(), Owner owner)
        {
            using(ApplicationContext db = new ApplicationContext())
            {
                Garage garage = new Garage
                {
                    Number = garageNumber,
                    SealNumber = sealNumber,
                    CounterNumber = counterNumber,
                    SealDate = sealDate,
                    Owner = owner
                };

                db.Garages.Add(garage);
                db.SaveChanges();
            }
        }

        public bool CanCreateNewOwner(Garage garage)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Owners.FirstOrDefault(o => o.Garages.Contains(garage)) is null;
            }
        }

        public void CreateNewOwner(string ownerName, decimal balance)
        {
            using(ApplicationContext db = new ApplicationContext())
            {
                Owner owner = new Owner
                {
                    Name = ownerName,
                    Balance = balance
                };

                db.Owners.Add(owner);
                db.SaveChanges();
            }
        }

        public bool CanLoadGarage(string garageNumber)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Garages.FirstOrDefault(g => string.Equals(g.Number, garageNumber)) is not null;
            }
        }

        public Garage LoadGarage(string garageNumber)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Garages.First(g => string.Equals(g.Number, garageNumber));
            }
        }

        public bool CanLoadOwner(string ownerName)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Owners.FirstOrDefault(o => string.Equals(o.Name, ownerName)) is not null;
            }
        }

        public Owner LoadOwner(Garage garage)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Owners.First(o => o.Garages.Contains(garage));
            }
        }

        public void SaveGarage(Garage garage)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Garages.Update(garage);
                db.SaveChanges();
            }
        }

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
