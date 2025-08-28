using System;
using System.Collections.Generic;

namespace BusinessManager.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int UomId { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal SalePrice { get; set; }

    public decimal PurchasePrice { get; set; }

    public int Stock { get; set; }

    public bool IsActive { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Uom Uom { get; set; } = null!;
}
