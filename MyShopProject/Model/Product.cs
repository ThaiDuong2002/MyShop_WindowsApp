namespace MyShopProject.Model;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double Weight { get; set; }

    public int Price { get; set; }

    public bool Status { get; set; }

    public int? BrandId { get; set; }

    public int PriceOriginal { get; set; }

    public int Quantity { get; set; }

    public string? Image { get; set; }


    public virtual Brand? Brand { get; set; }


    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

}
