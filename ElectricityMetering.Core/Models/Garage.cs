using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Models
{
    public class Garage
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;

        public Owner Owner { get; set; }
        public Counter Counter { get; set; }
        public Seal Seal { get; set; }
    }
}
