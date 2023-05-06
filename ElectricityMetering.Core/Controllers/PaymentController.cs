using ElectricityMetering.Core.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityMetering.Core.Controllers
{
    public class PaymentController : Controller
    {
        public string BlockOfGarages { get; set; }
        public decimal Cash { get; set; }
        public decimal NoneCash { get; set; }
        public DateOnly Date { get; set; } = new DateOnly();

        public async void AddPaymentAsync(string garageNumber, string cash, string noneCash)
        {
            _garage = await LoadGarageAsync(int.Parse(garageNumber));
            _garages = _repository.GetBlockOfGarages(_garage);

            BlockOfGarages = SplitBlockOfGarage();

            Cash = decimal.Parse(cash);
            NoneCash = decimal.Parse(noneCash);

            Date = DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
