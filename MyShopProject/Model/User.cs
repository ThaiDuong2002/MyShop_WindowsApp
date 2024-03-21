using System;
using System.Collections.Generic;

namespace MyShopProject.Model;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public byte Role { get; set; }

    public bool IsDisabled { get; set; }

    public DateOnly Birthday { get; set; }

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
