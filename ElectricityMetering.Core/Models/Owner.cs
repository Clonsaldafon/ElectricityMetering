using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Balance { get; set; }
        public List<Garage> Garages { get; set; } = new List<Garage>();
        public List<Payment> Payments { get; set; } = new List<Payment>();

        public Owner()
        {
            
        }
        
        public Owner(string name)
        {
            Name = name;
        }
    }
}
