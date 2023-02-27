using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.BL.Model
{
    public class Owner
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public List<Garage> Garages { get; set; } = null!;
        public int Balance { get; set; }
    }
}
