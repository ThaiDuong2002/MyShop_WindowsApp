using MyShopProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopProject.Repositories
{
    public class ProductCatgoryRepository
    {
        public ProductCatgoryRepository() { }

        public ObservableCollection<Brand> GetAllBrands()
        {
            using (var context = new MyShopContext())
            {
                return new ObservableCollection<Brand>(context.Brands.ToList());
            }
        }
    }
}
