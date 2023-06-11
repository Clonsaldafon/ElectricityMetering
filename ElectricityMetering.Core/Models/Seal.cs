using System.Globalization;

namespace ElectricityMetering.Core.Models
{
    public class Seal
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateOnly Date { get; set; } = DateOnly.Parse("01.01.1970", CultureInfo.InvariantCulture, DateTimeStyles.None);
        public List<Garage> Garages { get; set; } = new List<Garage>();

        public Seal()
        {

        }

        public Seal(string number)
        {
            Number = number;
        }
        
        public Seal(string number, DateOnly date)
        {
            Number = number;
            Date = date;
        }
    }
}
