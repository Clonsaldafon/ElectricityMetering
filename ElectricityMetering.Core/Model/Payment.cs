using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Model
{
    public class Payment
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public decimal Cash { get; set; }
        public decimal NonCash { get; set; }
        public decimal Total { get; set; }
        public Owner? Owner { get; set; }
    }
}
