using ElectricityMetering.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectricityMetering.Core
{
    public static class Repository
    {
        private static readonly ApplicationContext _context = new ApplicationContext();

        #region Create
        /// <summary>
        /// Create new garage and add it to the database if owner, seal and counter are null.
        /// </summary>
        /// <param name="garageNumber">Garage number from TextBox.</param>
        /// <returns>Created garage.</returns>
        public static async Task<Garage> CreateGarageAsync(int garageNumber)
        {
            Owner owner = await _context.Owners.FirstAsync(o => o.Id == 1);
            Counter counter = await _context.Counters.FirstAsync(c => c.Id == 1);
            Seal seal = await _context.Seals.FirstAsync(s => s.Id == 1);

            Garage garage = new Garage(garageNumber, owner, counter, seal);

            await _context.Garages.AddAsync(garage);
            await _context.SaveChangesAsync();

            return garage;
        }

        /// <summary>
        /// Create new owner and add it to the database.
        /// </summary>
        /// <param name="ownerName">Name of this owner from TextBox.</param>
        /// <param name="balance">Balance of this owner from TextBox.</param>
        /// <returns>Created owner.</returns>
        public static async Task<Owner> CreateOwnerAsync(string ownerName)
        {
            Owner owner = new Owner(ownerName);

            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();

            return owner;
        }

        /// <summary>
        /// Create new counter and add it to the database.
        /// </summary>
        /// <param name="counterNumber">Number of this counter from TextBox.</param>
        /// <returns>Created counter.</returns>
        public static async Task<Counter> CreateCounterAsync(string counterNumber)
        {
            Counter counter = new Counter(counterNumber);

            await _context.Counters.AddAsync(counter);
            await _context.SaveChangesAsync();

            return counter;
        }

        /// <summary>
        /// Create new seal and add it to the database.
        /// </summary>
        /// <param name="sealNumber">Number of this seal from TextBox.</param>
        /// <returns>Created seal.</returns>
        public static async Task<Seal> CreateSealAsync(string sealNumber, DateOnly sealDate)
        {
            Seal seal = new Seal(sealNumber, sealDate);

            await _context.Seals.AddAsync(seal);
            await _context.SaveChangesAsync();

            return seal;
        }

        public static async Task<Payment> CreatePaymentAsync(DateOnly date, decimal cash, decimal noneCash, Owner owner)
        {
            Payment payment = new Payment(date, cash, noneCash, owner);

            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public static async Task CreateTariffAsync(DateOnly date, decimal price)
        {
            Tariff tariff = new Tariff(price, date);

            await _context.Tariffs.AddAsync(tariff);
            await _context.SaveChangesAsync();
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
        public static async Task<Garage> SaveGarageAsync(Garage garage, Owner? owner, Counter? counter, Seal? seal)
        {
            owner ??= await GetOwnerAsync(1);
            counter ??= await GetCounterAsync(1);
            seal ??= await GetSealAsync(1);

            garage.Owner = owner;
            garage.Counter = counter;
            garage.Seal = seal;

            _context.Garages.Update(garage);
            await _context.SaveChangesAsync();

            return garage;
        }

        public static async Task SaveOwnerAsync(Owner owner, Payment payment)
        {
            owner.Balance += payment.Cash + payment.NoneCash;

            _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
        }

        public static async Task SaveCounterAsync(Counter counter)
        {
            _context.Counters.Update(counter);
            await _context.SaveChangesAsync();
        }

        public static async Task SaveIndicationsAsync(Counter counter, int[] indications)
        {
            counter.IndicationsNow = indications;

            _context.Counters.Update(counter);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Get
        /// <summary>
        /// Load the garage from the database by number.
        /// </summary>
        /// <param name="garageNumber">Number of this garage from TextBox.</param>
        /// <returns>Loaded garage.</returns>
        public static async Task<Garage?> GetGarageAsync(int garageNumber)
        {
            Garage? garage = await _context.Garages
                .Include(g => g.Owner)
                .Include(g => g.Counter)
                .Include(g => g.Seal)
                .Where(g => g.Number == garageNumber)
                .FirstOrDefaultAsync();

            return garage;
        }

        /// <summary>
        /// Load the garage from the database by owner.
        /// </summary>
        /// <param name="owner">Owner of this garage.</param>
        /// <returns>Loaded garage.</returns>
        public static async Task<Garage?> GetGarageAsync(Owner owner)
        {
            Garage? garage = await _context.Garages
                .Include(g => g.Owner)
                .Include(g => g.Counter)
                .Include(g => g.Seal)
                .Where(g => g.Owner == owner)
                .FirstOrDefaultAsync();
            return garage;
        }

        public static List<Garage> GetAllGarages()
        {
            using (ApplicationContext context = new ApplicationContext())
            {
                List<Garage> garages = context.Garages.OrderBy(g => g.Number).ToList();

                return garages;
            }
        }

        /// <summary>
        /// Load the owner from the database by name.
        /// </summary>
        /// <param name="ownerName">The owner's name.</param>
        /// <returns>Loaded owner.</returns>
        public static async Task<Owner?> GetOwnerAsync(string ownerName)
        {
            return await _context.Owners.FirstOrDefaultAsync(o => o.Name == ownerName);
        }

        /// <summary>
        /// Load the owner from the database by garage.
        /// </summary>
        /// <param name="garage">The owner's garage.</param>
        /// <returns>Loaded owner.</returns>
        public static async Task<Owner?> GetOwnerAsync(Garage garage)
        {
            return await _context.Owners.FirstOrDefaultAsync(o => o.Garages.Contains(garage));
        }

        /// <summary>
        /// Load the owner from the database by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Loaded owner.</returns>
        public static async Task<Owner> GetOwnerAsync(int id)
        {
            return await _context.Owners.FirstAsync(o => o.Id == id);
        }

        public static List<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        /// <summary>
        /// Load the counter from the database by garage.
        /// </summary>
        /// <param name="garage">The counter's garage.</param>
        /// <returns>Loaded counter.</returns>
        public static async Task<Counter?> GetCounterAsync(Garage garage)
        {
            return await _context.Counters.FirstOrDefaultAsync(c => c.Garages.Contains(garage));
        }

        /// <summary>
        /// Load the counter from the database by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Loaded counter.</returns>
        public static async Task<Counter> GetCounterAsync(int id)
        {
            return await _context.Counters.FirstAsync(c => c.Id == id);
        }

        /// <summary>
        /// Load the counter from the database by number.
        /// </summary>
        /// <param name="counterNumber">Number of this counter.</param>
        /// <returns>Loaded counter.</returns>
        public static async Task<Counter?> GetCounterAsync(string counterNumber)
        {
            return await _context.Counters.FirstOrDefaultAsync(c => c.Number == counterNumber);
        }

        /// <summary>
        /// Load the seal from the database by garage.
        /// </summary>
        /// <param name="garage">The seal's garage.</param>
        /// <returns>Loaded seal.</returns>
        public static async Task<Seal?> GetSealAsync(Garage garage)
        {
            return await _context.Seals.FirstOrDefaultAsync(s => s.Garages.Contains(garage));
        }

        /// <summary>
        /// Load the seal from the database by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Loaded owner.</returns>
        public static async Task<Seal> GetSealAsync(int id)
        {
            return await _context.Seals.FirstAsync(s => s.Id == id);
        }

        /// <summary>
        /// Load the seal from the database by number.
        /// </summary>
        /// <param name="sealNumber">Number of this seal.</param>
        /// <returns>Loaded owner.</returns>
        public static async Task<Seal?> GetSealAsync(string sealNumber)
        {
            return await _context.Seals.FirstOrDefaultAsync(s => s.Number == sealNumber);
        }

        /// <summary>
        /// Load list of garages associated with the current garage.
        /// </summary>
        /// <param name="garage">Current garage.</param>
        /// <returns>Loaded garages.</returns>
        public static List<Garage> GetBlockOfGarages(Garage garage)
        {
            return _context.Garages.Where(g => g.Owner == garage.Owner).OrderBy(g => g.Number).ToList();
        }

        public static List<Garage> GetBlockOfGarages(Owner owner)
        {
            /*return await _context.Garages.Where(g => g.Owner == owner).OrderBy(g => g.Number).ToListAsync();*/

            using (var context = new ApplicationContext())
            {
                return context.Garages
                    .Where(g => g.Owner == owner)
                    .OrderBy(g => g.Number)
                    .ToList();
            }
        }

        public static List<Payment> GetPayments()
        {
            return _context.Payments.Include(p => p.Owner).OrderByDescending(p => p.Date).ToList();
        }

        public static List<Tariff> GetTariffs()
        {
            return _context.Tariffs.OrderByDescending(t => t.Date).ToList();
        }

        public static List<Counter> GetCounters()
        {
            return _context.Counters.ToList();
        }
        #endregion
    }
}
