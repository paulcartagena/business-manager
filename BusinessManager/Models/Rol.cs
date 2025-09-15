using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Models;

[Table("rol")]
public partial class Rol
{
    [Key]
    [Column("rol_id")]
    public int RolId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [InverseProperty("Rol")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
