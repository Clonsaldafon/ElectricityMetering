using Microsoft.EntityFrameworkCore;
using ElectricityMetering.Core.Models;

namespace ElectricityMetering.Core
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Garage> Garages { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Counter> Counters { get; set; }
        public DbSet<Seal> Seals { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<BalanceRate> BalanceRates { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }

        private string _connectionString = $"Server=satao.db.elephantsql.com;Port=5432;Database=bowasjim;User Id=bowasjim;Password=9Cvmf5C8U79RKZ_madz-bs0PywwziGFl";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }
    }
}