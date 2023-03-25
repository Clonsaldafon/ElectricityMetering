using ElectricityMetering.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace ElectricityMetering.Core.Controller
{
    public class Repository
    {
        #region Create
        /// <summary>
        /// Create new garage and add it to the database if owner, seal and counter are null.
        /// </summary>
        /// <param name="garageNumber">Garage number from TextBox.</param>
        /// <returns>Created garage.</returns>
        public async Task<Garage> CreateGarageAsync(string garageNumber)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Garage garage = new Garage
                {
                    Number = garageNumber,
                    Owner = await db.Owners.FirstAsync(o => o.Name == "-"),
                    Counter = await db.Counters.FirstAsync(c => c.Number == "-"),
                    Seal = await db.Seals.FirstAsync(s => s.Number == "-"),
                };

                await db.Garages.AddAsync(garage);
                await db.SaveChangesAsync();

                return garage;
            }
        }

        /// <summary>
        /// Create new owner and add it to the database.
        /// </summary>
        /// <param name="ownerName">Name of this owner from TextBox.</param>
        /// <param name="balance">Balance of this owner from TextBox.</param>
        /// <returns>Created owner.</returns>
        public async Task<Owner> CreateOwnerAsync(string ownerName, decimal balance)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Owner owner = new Owner
                {
                    Name = ownerName,
                    Balance = balance
                };

                await db.Owners.AddAsync(owner);
                await db.SaveChangesAsync();

                return owner;
            }
        }

        /// <summary>
        /// Create new counter and add it to the database.
        /// </summary>
        /// <param name="counterNumber">Number of this counter from TextBox.</param>
        /// <returns>Created counter.</returns>
        public async Task<Counter> CreateCounterAsync(string counterNumber)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Counter counter = new Counter
                {
                    Number = counterNumber
                };

                await db.Counters.AddAsync(counter);
                await db.SaveChangesAsync();

                return counter;
            }
        }

        /// <summary>
        /// Create new seal and add it to the database.
        /// </summary>
        /// <param name="sealNumber">Number of this seal from TextBox.</param>
        /// <returns>Created seal.</returns>
        public async Task<Seal> CreateSealAsync(string sealNumber, DateOnly sealDate)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Seal seal = new Seal
                {
                    Number = sealNumber,
                    Date = sealDate
                };

                await db.Seals.AddAsync(seal);
                await db.SaveChangesAsync();

                return seal;
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Save garage data.
        /// </summary>
        /// <param name="garage">Current garage.</param>
        /// <param name="owner">Owner of this garage.</param>
        /// <param name="counter">Counter of this garage.</param>
        /// <param name="seal">Seal of this garage.</param>
        /// <returns></returns>
        public async Task SaveGarageAsync(Garage garage, Owner owner, Counter counter, Seal seal)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                garage.Owner = owner;
                garage.Counter = counter;
                garage.Seal = seal;

                db.Garages.Update(garage);
                await db.SaveChangesAsync();
            }
        }
        #endregion

        #region Get
        /// <summary>
        /// Load the garage from the database by number.
        /// </summary>
        /// <param name="garageNumber">Number of this garage from TextBox.</param>
        /// <returns>Loaded garage.</returns>
        public async Task<Garage?> GetGarageAsync(string garageNumber)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Garage? garage = await db.Garages
                    .Include(g => g.Owner)
                    .Include(g => g.Counter)
                    .Include(g => g.Seal)
                    .Where(g => g.Number == garageNumber)
                    .FirstOrDefaultAsync();
                return garage;
            }
        }

        /// <summary>
        /// Load the garage from the database by owner.
        /// </summary>
        /// <param name="owner">Owner of this garage.</param>
        /// <returns>Loaded garage.</returns>
        public async Task<Garage?> GetGarageAsync(Owner owner)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Garage? garage = await db.Garages
                    .Include(g => g.Owner)
                    .Include(g => g.Counter)
                    .Include(g => g.Seal)
                    .Where(g => g.Owner == owner)
                    .FirstOrDefaultAsync();
                return garage;
            }
        }

        /// <summary>
        /// Load the owner from the database by garage.
        /// </summary>
        /// <param name="garage">The owner's garage.</param>
        /// <returns>Loaded owner.</returns>
        public async Task<Owner?> GetOwnerAsync(Garage garage)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Owners.FirstOrDefaultAsync(o => o.Garages.Contains(garage));
            }
        }

        /// <summary>
        /// Load the owner from the database by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Loaded owner.</returns>
        public async Task<Owner?> GetOwnerAsync(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Owners.FirstOrDefaultAsync(o => o.Id == id);
            }
        }

        /// <summary>
        /// Load the counter from the database by garage.
        /// </summary>
        /// <param name="garage">The counter's garage.</param>
        /// <returns>Loaded counter.</returns>
        public async Task<Counter?> GetCounterAsync(Garage garage)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Counters.FirstOrDefaultAsync(c => c.Garages.Contains(garage));
            }
        }

        /// <summary>
        /// Load the counter from the database by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Loaded counter.</returns>
        public async Task<Counter?> GetCounterAsync(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Counters.FirstOrDefaultAsync(c => c.Id == id);
            }
        }

        /// <summary>
        /// Load the seal from the database by garage.
        /// </summary>
        /// <param name="garage">The seal's garage.</param>
        /// <returns>Loaded seal.</returns>
        public async Task<Seal?> GetSealAsync(Garage garage)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Seals.FirstOrDefaultAsync(s => s.Garages.Contains(garage));
            }
        }

        /// <summary>
        /// Load the seal from the database by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Loaded owner.</returns>
        public async Task<Seal?> GetSealAsync(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Seals.FirstOrDefaultAsync(s => s.Id == id);
            }
        }

        /// <summary>
        /// Load list of garages associated with the current garage.
        /// </summary>
        /// <param name="garage">Current garage.</param>
        /// <returns>Loaded garages.</returns>
        public List<Garage> GetBlockOfGarages(Garage garage)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Garages.Where(g => g.Owner == garage.Owner).OrderBy(g => g.Number).ToList();
            }
        }
        #endregion
    }
}
