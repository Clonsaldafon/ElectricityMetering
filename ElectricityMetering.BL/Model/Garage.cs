using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.BL.Model
{
    public class Garage
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public string? SealNumber { get; set; }
        public string? CounterNumber { get; set; }
        public DateOnly SealingDate { get; set; }
        public Indication? Indication { get; set; }
    }
}
