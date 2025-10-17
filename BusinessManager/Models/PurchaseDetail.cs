using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessManager.Models
{
    [Table("purchase_detail")]
    public class PurchaseDetail
    {
        [Key]
        [Column("purchase_detail_id")]
        public int PurchaseDetailId { get; set; }

        [Column("purchase_id")]
        public int PurchaseId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("unit_price")]
        public decimal UnitPrice { get; set; }

        [Column("sub_total")]
        public decimal SubTotal { get; set; }

        [ForeignKey("PurchaseId")]
        [InverseProperty("PurchaseDetails")]
        public virtual Purchase Purchase { get; set; } = null!;

        [ForeignKey("ProductId")]
        [InverseProperty("PurchaseDetails")]
        public virtual Product Product { get; set; } = null!;
    }
}
