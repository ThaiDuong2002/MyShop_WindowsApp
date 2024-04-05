using Microsoft.EntityFrameworkCore;
using MyShopProject.Model;
using System.Collections.ObjectModel;

namespace MyShopProject.Repositories
{
    public class ProductRepository
    {
        public ObservableCollection<Product> GetAllProducts()
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            using (var context = new MyShopContext())
            {
                products = new ObservableCollection<Product>(context.Products
                    .Include(p => p.Brand)
                    .ToList());
            }
            return products;
        }

        public (int totalCount, ObservableCollection<Product> products) GetFilteredProducts(int category, int page, int itemPerPage, int? sortOrder, string? name, double? minPrice, double? maxPrice)
        {
            int totalCount;
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            using (var context = new MyShopContext())
            {
                IQueryable<Product> query = context.Products.Include(p => p.Brand);

                // Lọc theo danh mục nếu category có giá trị
                if (category != 0)
                    query = query.Where(p => p.BrandId == category);

                // Sắp xếp theo giá nếu sortOrder là "asc", ngược lại sắp xếp giảm dần
                if (sortOrder.HasValue)
                {
                    if (sortOrder == 0)
                        query = query.OrderBy(p => p.Price);
                    else if (sortOrder == 1)
                        query = query.OrderByDescending(p => p.Price);
                }

                // Lọc theo tên nếu name có giá trị
                if (!string.IsNullOrEmpty(name))
                    query = query.Where(p => p.Name.Contains(name));

                // Lọc theo giá nếu minPrice và maxPrice có giá trị
                if (minPrice.HasValue && maxPrice.HasValue)
                    query = query.Where(p => p.Price >= minPrice && p.Price <= maxPrice);

                // Đếm số lượng sản phẩm sau khi lọc
                totalCount = query.Count();

                // Bỏ qua và lấy số lượng item tương ứng cho trang cụ thể
                products = new ObservableCollection<Product>(query
                    .Skip((page - 1) * itemPerPage)
                    .Take(itemPerPage)
                    .ToList());
            }
            return (totalCount, products);
        }

        public Product getProductByID(int id)
        {
            Product product = new Product();
            using (var context = new MyShopContext())
            {
                product = context.Products
                    .Include(p => p.Brand)
                    .Where(p => p.Id == id)
                    .FirstOrDefault();
            }
            return product;
        }
        public ObservableCollection<Product> getProductsByBrand(int brand)
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            using (var context = new MyShopContext())
            {
                products = new ObservableCollection<Product>(context.Products
                    .Include(p => p.Brand)
                    .Where(p => p.BrandId == brand)
                    .ToList());
            }
            return products;
        }

        public int getNumOfProductsAvailable()
        {
            // các sản phẩm có status là true
            int count = 0;
            using (var context = new MyShopContext())
            {
                count = context.Products.Where(p => p.Status == true).Count();
            }
            return count;
        }
        public int getNumOfProductsByBrand(int category)
        {
            // các sản phẩm có status là true
            int count = 0;
            using (var context = new MyShopContext())
            {
                count = context.Products.Where(p => p.Status == true && p.BrandId == category).Count();
            }
            return count;
        }
        public ObservableCollection<Product> getTop5Product()
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            using (var context = new MyShopContext())
            {
                products = new ObservableCollection<Product>(context.Products
                    .Where(p => p.Quantity < 5)
                    .OrderBy(p => p.Quantity)
                    .Take(5)
                    .ToList());
            }
            return products;
        }
        public double getMaxPrice()
        {
            double maxPrice = 0;
            using (var context = new MyShopContext())
            {
                maxPrice = context.Products.Max(p => p.Price);
            }
            return maxPrice;
        }


        //write function CRUD here
        public bool AddProduct(Product product)
        {
            using (var context = new MyShopContext())
            {
                context.Products.Add(product);
                context.SaveChanges();
                return true;
            }
        }
        public bool UpdateProduct(Product product, int id)
        {
            using (var context = new MyShopContext())
            {
                var productUpdate = context.Products.Where(u => u.Id == id).FirstOrDefault();
                if (productUpdate != null)
                {
                    productUpdate.Name = product.Name;
                    productUpdate.WarrantyPeriod = product.WarrantyPeriod;
                    productUpdate.Weight = product.Weight;
                    productUpdate.Price = product.Price;
                    productUpdate.BrandId = product.BrandId;
                    productUpdate.Quantity = product.Quantity;
                    productUpdate.Image = product.Image;
                }
                context.SaveChanges();
                return true;
            }
        }
        public bool DeleteProduct(int id)
        {
            using (var context = new MyShopContext())
            {
                var product = context.Products.Find(id);
                context.Products.Remove(product);
                context.SaveChanges();
                return true;
            }
        }
        public bool UpdateQuantityAfterOrder(int id, int quantity)
        {
            using (var context = new MyShopContext())
            {
                var product = context.Products.Find(id);
                product!.Quantity -= quantity;
                context.SaveChanges();
                return true;
            }
        }
        public bool CheckProductQuantity(int id, int quantity)
        {
            using (var context = new MyShopContext())
            {
                var product = context.Products.Find(id);
                if (product!.Quantity < quantity)
                    return false;
                return true;
            }
        }
    }
}
