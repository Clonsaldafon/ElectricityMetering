using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Model
{
    public class PricePerKw
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public DateOnly Date { get; set; }
    }
}
