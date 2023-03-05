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
        public decimal Debt { get; set; }
        public decimal Advance { get; set; }
        public decimal Balance { get; set; }
    }
}
