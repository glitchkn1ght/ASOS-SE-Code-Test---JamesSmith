namespace App.Models
{
    using App.Enums;
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Classification Classification { get; set; }
    }
}