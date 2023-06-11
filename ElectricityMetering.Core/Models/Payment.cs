namespace ElectricityMetering.Core.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public decimal Cash { get; set; }
        public decimal NoneCash { get; set; }
        public Owner Owner { get; set; }

        public Payment()
        {
            
        }

        public Payment(DateOnly date, decimal cash, decimal noneCash, Owner owner)
        {
            Date = date;
            Cash = cash;
            NoneCash = noneCash;
            Owner = owner;
        }
    }
}
