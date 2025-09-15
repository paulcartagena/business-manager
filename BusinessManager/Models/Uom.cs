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
    [Column("uom_id")]
    public int UomId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("is_weight_unit")]
    public bool IsWeightUnit { get; set; }

    [InverseProperty("Uom")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
