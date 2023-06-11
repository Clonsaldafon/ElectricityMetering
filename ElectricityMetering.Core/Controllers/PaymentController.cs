using ElectricityMetering.Core.Models;

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

        /// <summary>
        /// Add a new payment to the database.
        /// </summary>
        /// <param name="garageNumber">Current garage number.</param>
        /// <param name="cashString">Amount of cash.</param>
        /// <param name="noneCashString">Amount of none cash.</param>
        /// <returns></returns>
        public async Task AddPaymentAsync(int garageNumber, string cashString, string noneCashString)
        {
            Garage? garage = await Repository.GetGarageAsync(garageNumber);

            if (garage == null)
            {
                return;
            }

            decimal cash = decimal.TryParse(cashString, out decimal cashResult) ? cashResult : 0;
            decimal noneCash = decimal.TryParse(noneCashString, out decimal noneCashResult) ? noneCashResult : 0;

            await Repository.CreatePaymentAsync(DateOnly.FromDateTime(DateTime.Now), cash, noneCash, garage.Owner);

            await _ownerController.UpdateBalanceAsync(garage.Owner);

            UpdatePayments();
        }

        private void UpdatePayments()
        {
            Payments = Repository.GetPayments();

            BlocksOfGarages = SplitAllBlockOfGarages();
        }
    }
}
