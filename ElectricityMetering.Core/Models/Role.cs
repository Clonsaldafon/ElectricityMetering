namespace ElectricityMetering.Core.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsAuth { get; set; }

        public Role()
        {
            
        }

        public Role(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
