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
        public Brand GetBrandById(int Id)
        {
            using (var context = new MyShopContext())
            {
                return context.Brands.Where(b => b.Id == Id).FirstOrDefault();
            }
        }
        public bool AddBrand(Brand brand)
        {
            using (var context = new MyShopContext())
            {
                context.Brands.Add(brand);
                context.SaveChanges();
                return true;
            }
        }

        public bool UpdateBrand(Brand brand,int Id)
        {
            using (var context = new MyShopContext())
            {
                var brandToUpdate = context.Brands.Where(b => b.Id == Id).FirstOrDefault();
                if (brandToUpdate != null)
                {
                    brandToUpdate.Name = brand.Name;
                    brandToUpdate.Logo = brand.Logo;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public bool DeleteBrand(int Id)
        {
            using (var context = new MyShopContext())
            {
                var brandToDelete = context.Brands.Where(b => b.Id == Id).FirstOrDefault();
                if (brandToDelete != null)
                {
                    context.Brands.Remove(brandToDelete);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
