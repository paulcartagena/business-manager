using System;
using System.Collections.Generic;

namespace BusinessManager.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public int RolId { get; set; }

    public virtual Rol Rol { get; set; } = null!;
}
