using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Model
{
    public class BalanceRate
    {
        public int Id { get; set; }
        public double Debt { get; set; }
        public double Advance { get; set; }
        public double Balance { get; set; }
    }
}
