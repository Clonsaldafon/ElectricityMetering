using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Models
{
    public class Tariff
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateOnly Date { get; set; }

        public Tariff()
        {
            
        }
        
        public Tariff(decimal price, DateOnly date)
        {
            Price = price;
            Date = date;
        }
    }
}
