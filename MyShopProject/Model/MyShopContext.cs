using Microsoft.EntityFrameworkCore;

namespace MyShopProject.Model;

public partial class MyShopContext : DbContext
{
    public MyShopContext()
    {
    }

    public MyShopContext(DbContextOptions<MyShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Cpu> Cpus { get; set; }

    public virtual DbSet<GraphicsCard> GraphicsCards { get; set; }

    public virtual DbSet<LaptopSeries> LaptopSeries { get; set; }

    public virtual DbSet<OS> Os { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Ram> Rams { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost; Trusted_Connection=Yes; Initial Catalog=MyShop; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("Brand");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(25);
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.ToTable("Color");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Cpu>(entity =>
        {
            entity.ToTable("CPU");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Generation).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<GraphicsCard>(entity =>
        {
            entity.ToTable("GraphicsCard");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<LaptopSeries>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Brand).WithMany(p => p.LaptopSeries)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_LaptopSeries_Brand");
        });

        modelBuilder.Entity<OS>(entity =>
        {
            entity.ToTable("OS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Order_User");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.ToTable("OrderProduct");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.PromotionId).HasColumnName("PromotionID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderProduct_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_OrderProduct_Product");

            entity.HasOne(d => d.Promotion).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.PromotionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_OrderProduct_Promotion");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BrandId).HasColumnName("BrandID");
            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.CpuId).HasColumnName("CPU_ID");
            entity.Property(e => e.GraphicsCardId).HasColumnName("GraphicsCardID");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.OsId).HasColumnName("OS_ID");
            entity.Property(e => e.PartNumber).HasMaxLength(50);
            entity.Property(e => e.RamId).HasColumnName("RAM_ID");
            entity.Property(e => e.StorageId).HasColumnName("StorageID");
            entity.Property(e => e.WarrantyPeriod).HasMaxLength(20);

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Product_Brand");

            entity.HasOne(d => d.Color).WithMany(p => p.Products)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Product_Color");

            entity.HasOne(d => d.Cpu).WithMany(p => p.Products)
                .HasForeignKey(d => d.CpuId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Product_CPU");

            entity.HasOne(d => d.GraphicsCard).WithMany(p => p.Products)
                .HasForeignKey(d => d.GraphicsCardId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Product_GraphicsCard");

            entity.HasOne(d => d.Os).WithMany(p => p.Products)
                .HasForeignKey(d => d.OsId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Product_OS");

            entity.HasOne(d => d.Ram).WithMany(p => p.Products)
                .HasForeignKey(d => d.RamId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Product_RAM");

            entity.HasOne(d => d.Storage).WithMany(p => p.Products)
                .HasForeignKey(d => d.StorageId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Product_Storage");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.ToTable("Promotion");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ByProduct).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Ram>(entity =>
        {
            entity.ToTable("RAM");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.ToTable("Storage");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
