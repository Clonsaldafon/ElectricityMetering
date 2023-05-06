using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public decimal Cash { get; set; }
        public decimal NonCash { get; set; }
        public Owner Owner { get; set; } = null!;
    }
}
