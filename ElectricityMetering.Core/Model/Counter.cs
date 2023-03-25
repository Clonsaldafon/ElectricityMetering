using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Model
{
    public class Counter
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public int[] Indications { get; set; } = new int[36];
        public List<Garage> Garages { get; set; }
    }
}
