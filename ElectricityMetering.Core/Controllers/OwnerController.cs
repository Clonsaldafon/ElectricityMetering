using ElectricityMetering.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controllers
{
    public class OwnerController : Controller
    {
        public async Task UpdateBalanceAsync(Owner owner, Payment payment)
        {
            if (owner != payment.Owner)
            {
                return;
            }

            await Repository.SaveOwnerAsync(owner, payment);
        }
    }
}
