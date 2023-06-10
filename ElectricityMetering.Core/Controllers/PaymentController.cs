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

        private readonly BalanceController _balanceController = new BalanceController();

        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<string> BlocksOfGarages { get; set; } = new List<string>();

        public PaymentController()
        {
            UpdatePayments();
        }

        public async Task AddPaymentAsync(int garageNumber, string cashString, string noneCashString)
        {
            _garage = await LoadGarageAsync(garageNumber);

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

            Payment payment = await Repository.CreatePaymentAsync(DateOnly.FromDateTime(DateTime.Now), cash, noneCash, _owner);

            await _ownerController.UpdateBalanceAsync(_owner);

            UpdatePayments();
        }

        private void UpdatePayments()
        {
            Payments = Repository.GetPayments();

            BlocksOfGarages = SplitAllBlockOfGarages();
        }
    }
}
