namespace MyShopProject.Model;

public partial class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<LaptopSeries> LaptopSeries { get; set; } = new List<LaptopSeries>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
