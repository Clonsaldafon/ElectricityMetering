using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Model
{
    public class Indication
    {
        public int Id { get; set; }
        public int[] Indications { get; set; } = new int[36];
    }
}
