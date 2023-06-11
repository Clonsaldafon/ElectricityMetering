namespace ElectricityMetering.Core.Models
{
    public class Garage
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public Owner Owner { get; set; }
        public Counter Counter { get; set; }
        public Seal Seal { get; set; }

        public Garage()
        {
            
        }
        
        public Garage(int number, Owner owner, Counter counter, Seal seal)
        {
            Number = number;
            Owner = owner;
            Counter = counter;
            Seal = seal;
        }
    }
}
