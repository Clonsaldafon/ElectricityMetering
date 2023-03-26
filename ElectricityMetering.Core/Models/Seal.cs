using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Models
{
    public class Seal
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public DateOnly Date { get; set; } = new DateOnly();
        public List<Garage> Garages { get; set; }
    }
}
