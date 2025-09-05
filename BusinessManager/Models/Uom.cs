using System;
using System.Collections.Generic;

namespace BusinessManager.Models;

public partial class Uom
{
    public int UomId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsWeightUnit { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
