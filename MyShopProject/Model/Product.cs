using System;
using System.Collections.Generic;

namespace MyShopProject.Model;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string WarrantyPeriod { get; set; } = null!;

    public string PartNumber { get; set; } = null!;

    public double Weight { get; set; }

    public int Price { get; set; }

    public bool Status { get; set; }

    public int? BrandId { get; set; }

    public int? ColorId { get; set; }

    public int? CpuId { get; set; }

    public int? GraphicsCardId { get; set; }

    public int? RamId { get; set; }

    public int? StorageId { get; set; }

    public int? OsId { get; set; }

    public int? Quantity { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Color? Color { get; set; }

    public virtual Cpu? Cpu { get; set; }

    public virtual GraphicsCard? GraphicsCard { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual OS? Os { get; set; }

    public virtual Ram? Ram { get; set; }

    public virtual Storage? Storage { get; set; }
}
