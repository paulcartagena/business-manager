using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Models;

[Table("product")]
[Index("CategoryId", Name = "IX_Product_categoryId")]
[Index("UomId", Name = "IX_Product_uomId")]
public partial class Product
{
    [Key]
    [Column("productId")]
    public int ProductId { get; set; }

    [Column("uomId")]
    public int UomId { get; set; }

    [Column("categoryId")]
    public int CategoryId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [StringLength(255)]
    public string? Description { get; set; }

    [Column("salePrice")]
    [Precision(10, 2)]
    public decimal SalePrice { get; set; }

    [Column("purchasePrice")]
    [Precision(10, 2)]
    public decimal PurchasePrice { get; set; }

    [Column("stock")]
    public int Stock { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("UomId")]
    [InverseProperty("Products")]
    public virtual Uom Uom { get; set; } = null!;
}
