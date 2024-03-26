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
        public ObservableCollection<Brand> GetAllBrands()
        {
            using (var context = new MyShopContext())
            {
                return new ObservableCollection<Brand>(context.Brands.ToList());
            }
        }

        public List<string> GetAllBrandsName()
        {
            using (var context = new MyShopContext())
            {
                return new List<string>(context.Brands.Select(b => b.Name).ToList());
            }
        }
        public ObservableCollection<Brand> GetBrandByPagination(int page, int itemPerPage)
        {
            using (var context = new MyShopContext())
            {
                return new ObservableCollection<Brand>(context.Brands.Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList());
            }

        }

        public int GetNumOfBrands()
        {
            using (var context = new MyShopContext())
            {
                return context.Brands.Count();
            }
        }
        public ObservableCollection<Brand> SearchBrandByName(string name)
        {
            using (var context = new MyShopContext())
            {
                return new ObservableCollection<Brand>(context.Brands.Where(b => b.Name.Contains(name)).ToList());
            }
        }
        public Brand GetBrandByName(string name)
        {
            using (var context = new MyShopContext())
            {
                return context.Brands.Where(b => b.Name == name).FirstOrDefault();
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

        public bool UpdateBrand(Brand brand, int Id)
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
