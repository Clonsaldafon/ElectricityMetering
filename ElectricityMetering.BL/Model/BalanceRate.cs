using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.BL.Model
{
    public class BalanceRate
    {
        public int Id { get; set; }
        public double Rate { get; set; }
        public DateOnly Date { get; set; }
    }
}
