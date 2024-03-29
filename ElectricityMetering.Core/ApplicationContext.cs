﻿using Microsoft.EntityFrameworkCore;
using ElectricityMetering.Core.Models;
using System.Globalization;

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
        public DbSet<Tariff> Tariffs { get; set; }

        private string _connectionString = "Server=satao.db.elephantsql.com;Port=5432;Database=bowasjim;User Id=bowasjim;Password=9Cvmf5C8U79RKZ_madz-bs0PywwziGFl";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Председатель", Password = "0000" },
                new Role { Id = 2, Name = "Электрик", Password = "qwerty" }
                );

            modelBuilder.Entity<Owner>().HasData(
                new Owner { Id = 1, Name = "-" }
                );

            modelBuilder.Entity<Counter>().HasData(
                new Counter { Id = 1, Number = "-" }
                );

            modelBuilder.Entity<Seal>().HasData(
                new Seal { Id = 1, Number = "-" }
                );

            modelBuilder.Entity<Tariff>().HasData(
                new Tariff { Id = 1, Price = 3.79M, Date = DateOnly.ParseExact("01.07.2019", "dd.MM.yyyy", CultureInfo.InvariantCulture) },
                new Tariff { Id = 2, Price = 3.82M, Date = DateOnly.ParseExact("01.01.2020", "dd.MM.yyyy", CultureInfo.InvariantCulture) },
                new Tariff { Id = 3, Price = 3.85M, Date = DateOnly.ParseExact("01.07.2020", "dd.MM.yyyy", CultureInfo.InvariantCulture) },
                new Tariff { Id = 4, Price = 3.86M, Date = DateOnly.ParseExact("01.01.2021", "dd.MM.yyyy", CultureInfo.InvariantCulture) },
                new Tariff { Id = 5, Price = 3.88M, Date = DateOnly.ParseExact("01.07.2021", "dd.MM.yyyy", CultureInfo.InvariantCulture) },
                new Tariff { Id = 6, Price = 3.91M, Date = DateOnly.ParseExact("01.01.2022", "dd.MM.yyyy", CultureInfo.InvariantCulture) },
                new Tariff { Id = 7, Price = 3.93M, Date = DateOnly.ParseExact("01.07.2022", "dd.MM.yyyy", CultureInfo.InvariantCulture) },
                new Tariff { Id = 8, Price = 3.95M, Date = DateOnly.ParseExact("01.01.2023", "dd.MM.yyyy", CultureInfo.InvariantCulture) }
                );
        }
    }
}