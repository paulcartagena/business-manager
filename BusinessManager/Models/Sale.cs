using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessManager.Models
{
    [Table("sale")]
    public class Sale
    {
        [Key]
        [Column("sale_id")]
        public int SaleId { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("sale_date")]
        public DateTime SaleDate { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("notes")]
        [StringLength(100)]
        public string? Notes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Sales")]
        public virtual Customer Customer { get; set; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("Sales")]
        public virtual User User { get; set; } = null!;

        [InverseProperty("Sale")]
        public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    }
}
