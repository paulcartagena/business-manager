namespace BusinessManager.Models
{
    public class Supplier
    {
        public int SuplierId { get; set; }

        public string Name { get; set; } = null!;

        public string ContactName { get; set; } = null!;

        public string? Phone {  get; set; }

        public string? Address { get; set; }

        public bool IsActive { get; set; }
    }
}
