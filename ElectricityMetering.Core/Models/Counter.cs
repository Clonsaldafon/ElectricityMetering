using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Models
{
    public class Counter
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public int[] Indications { get; set; } = new int[36];
        public List<Garage> Garages { get; set; }

        public Counter()
        {
            
        }

        public Counter(string number)
        {
            Number = number;
        }
    }
}
