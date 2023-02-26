using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.BL.Model
{
    public class Payment
    {
        public int Id { get; set; }
        public double Cash { get; set; }
        public double NonCash { get; set; }
        public double Total { get; set; }
        public Owner Owner { get; set; } = null!;
    }
}
