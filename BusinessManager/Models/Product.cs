using BusinessManager.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessManager.Models;

[Table("product")]
[Index("CategoryId", Name = "IX_Product_categoryId")]
[Index("UomId", Name = "IX_Product_uomId")]
public partial class Product
{
    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("uom_id")]
    public int UomId { get; set; }

    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(100)]
    public string? Description { get; set; }

    [Column("sale_price")]
    [Precision(10, 2)]
    public decimal SalePrice { get; set; }

    [NotMapped]
    public int CurrentStock
    {
        get
        {
            if (InventoryMovements == null || !InventoryMovements.Any())
                return 0;
            int stock = 0;
            foreach (var movement in InventoryMovements)
            {
                if (movement.MovementType == MovementTypeEnum.ENTRADA)
                    stock += movement.Quantity;
                else if (movement.MovementType == MovementTypeEnum.SALIDA)
                    stock -= movement.Quantity;
                else if (movement.MovementType == MovementTypeEnum.AJUSTE)
                    stock += movement.Quantity;
            }
            return stock;
        }
    }

    [NotMapped]
    public decimal AveragePurchasePrice
    {
        get
        {
            if (PurchaseDetails == null || !PurchaseDetails.Any())
                return 0;

            return PurchaseDetails.Average(pd => pd.UnitPrice);
        }
    }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("UomId")]
    [InverseProperty("Products")]
    public virtual Uom Uom { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();

    [InverseProperty("Product")]
    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    [InverseProperty("Product")]
    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}