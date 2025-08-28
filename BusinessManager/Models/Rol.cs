using System;
using System.Collections.Generic;

namespace BusinessManager.Models;

public partial class Rol
{
    public int RolId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
