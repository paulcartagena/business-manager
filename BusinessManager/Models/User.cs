using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BusinessManager.Models;

[Table("user")]
[Index("RolId", Name = "IX_User_rolId")]
public partial class User
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("mail")]
    [StringLength(100)]
    public string Mail { get; set; } = null!;

    [Column("rol_id")]
    public int RolId { get; set; }

    [ForeignKey("RolId")]
    [InverseProperty("Users")]
    public virtual Rol Rol { get; set; } = null!;
}
