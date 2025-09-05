using System.Numerics;

namespace BusinessManager.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }

        public int CustomerId { get; set; }

        public int UserId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; } = null!;

        public virtual User User { get; set; } = null!; 
    }
}
