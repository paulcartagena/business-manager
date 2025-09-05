using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Models;

[Table("uom")]
public partial class Uom
{
    [Key]
    [Column("uomId")]
    public int UomId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("isWeightUnit")]
    public bool IsWeightUnit { get; set; }

    [InverseProperty("Uom")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
