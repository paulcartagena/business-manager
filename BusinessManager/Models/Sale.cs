namespace BusinessManager.Models
{
    public class Sale
    {
        public int SaleId { get; set; }

        public int CustomerId { get; set; }

        public int UserId { get; set; }

        public DateTime SaleDate { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
