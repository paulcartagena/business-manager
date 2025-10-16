using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace BusinessManager.Models
{
    [Table("purchase")]
    public class Purchase
    {
        [Key]
        [Column("purchase_id")]
        public int PurchaseId { get; set; }

        [Column("supplier_id")]
        public int SupplierId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("purchase_date")]
        public DateTime PurchaseDate { get; set; }

        [Column("total_amount")]
        public decimal TotalAmount { get; set; }

        [Column("notes")]
        [StringLength(100)]
        public string? Notes { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey("SupplierId")]
        [InverseProperty("Purchases")]
        public virtual Supplier Supplier { get; set; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("Purchases")]
        public virtual User User { get; set; } = null!; 
    }
}
