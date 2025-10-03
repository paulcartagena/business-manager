using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessManager.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Models
{
    [Table("movement_inventory")]
    public partial class InventoryMovement
    {
        [Key]
        [Column("movement_inventory_id")]
        public int InventoryMovementId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [Column("movement_type")]
        public MovementTypeEnum MovementType { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("notes")]
        [StringLength(100)]
        public string? Notes { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("InventoryMovements")]
        public virtual Product Product { get; set; } = null!;
    }
}

