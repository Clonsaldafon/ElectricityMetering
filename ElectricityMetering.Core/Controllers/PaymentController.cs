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
        private readonly OwnerController _ownerController = new OwnerController();

        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<string> BlocksOfGarages { get; set; } = new List<string>();

        public PaymentController()
        {
            UpdatePayments();
        }

        public async void AddPaymentAsync(string garageNumber, string cashString, string noneCashString)
        {
            _garage = await LoadGarageAsync(int.Parse(garageNumber));

            FillDataByGarage();

            decimal cash, noneCash;

            if (!decimal.TryParse(cashString, out cash))
            {
                cash = 0;
            }
            else
            {
                cash = decimal.Parse(cashString);
            }

            if (!decimal.TryParse(noneCashString, out noneCash))
            {
                noneCash = 0;
            }
            else
            {
                noneCash = decimal.Parse(noneCashString);
            }

            Payment payment = await _repository.CreatePaymentAsync(DateOnly.FromDateTime(DateTime.Now), cash, noneCash, _owner);

            await _ownerController.UpdateBalanceAsync(_owner, payment);

            UpdatePayments();
        }

        private void UpdatePayments()
        {
            Payments = _repository.GetPayments();

            BlocksOfGarages = SplitAllBlockOfGarages();
        }
    }
}
