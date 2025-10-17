using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessManager.Models
{
    [Table("sale_detail")]
    public class SaleDetail
    {
        [Key]
        [Column("sale_detail_id")]
        public int SaleDetailId { get; set; }

        [Column("sale_id")]
        public int SaleId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("unit_price")]
        public decimal UnitPrice { get; set; }

        [Column("sub_total")]
        public decimal SubTotal { get; set; }

        [ForeignKey("SaleId")]
        [InverseProperty("SaleDetails")]
        public virtual Sale Sale { get; set; } = null!;

        [ForeignKey("ProductId")]
        [InverseProperty("SaleDetails")]
        public virtual Product Product { get; set; } = null!;
    }
}
