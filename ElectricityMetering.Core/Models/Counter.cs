namespace ElectricityMetering.Core.Models
{
    public class Counter
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public int[] IndicationsNow { get; set; } = new int[12];
        public int[] IndicationsOneYearAgo { get; set; } = new int[12];
        public int[] IndicationsTwoYearsAgo { get; set; } = new int[12];
        public List<Garage> Garages { get; set; } = new List<Garage>();

        public Counter()
        {
            
        }

        public Counter(string number)
        {
            Number = number;
        }
    }
}
