using MyShopProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.repositories
{
    public class ProductRepository
    {
        public ObservableCollection<Product> GetAllProducts()
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            using (var context = new MyShopContext())
            {
                products = new ObservableCollection<Product>(context.Products.ToList());
            }
            return products;
        }
        public ObservableCollection<Product> GetProductsByCategory(int category)
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            using (var context = new MyShopContext())
            {
                products = new ObservableCollection<Product>(context.Products.Where(p => p.BrandId == category).ToList());
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
        public ObservableCollection<Product> getTop5Product()
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            using (var context = new MyShopContext())
            {
                products = new ObservableCollection<Product>(context.Products.Where(p=>p.Quantity < 5).ToList());
            }
            products = new ObservableCollection<Product>(products.OrderBy(p => p.Quantity).Take(5).ToList());
            return products;
        }
    }
}
