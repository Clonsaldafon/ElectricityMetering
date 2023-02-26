using ElectricityMetering.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.BL.Controller
{
    public class Loader
    {
        public void LoadInfo(int garageId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Garage? garage = db.Garages.FirstOrDefault(g => g.Id == garageId);
                if (garage is not null)
                {
                    Owner? owner = db.Owners.FirstOrDefault(o => o.Garages.Contains(garage));
                }
                
            }
        }
    }
}
