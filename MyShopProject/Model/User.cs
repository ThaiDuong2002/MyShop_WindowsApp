using System;
using System.Collections.Generic;

namespace MyShopProject.Model;

public partial class User : ICloneable
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly Birthday { get; set; } = new DateOnly(2024, 1, 1);

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;
    public string? Avatar { get; set; } 

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public object Clone()
    {
        return MemberwiseClone();
    }
}
