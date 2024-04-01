namespace MyShopProject.Model;

public partial class Promotion
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ByPercent { get; set; }

    public int? ByCash { get; set; }

    public string? ByProduct { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
