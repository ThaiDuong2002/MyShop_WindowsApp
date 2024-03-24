using MyShopProject.Model;
using MyShopProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MyShopProject.ViewModel
{
    public class EditBrandViewModel : BaseViewModel
    {
        public ProductCatgoryRepository ProductCatgoryRepository;
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        private byte[] _logo;
        public byte[] Logo
        {
            get { return _logo; }
            set
            {
                _logo = value;
                OnPropertyChanged();
            }
        }
        public int Id;
        public ICommand SaveCommand { get; set; }
        public EditBrandViewModel()
        {
            ProductCatgoryRepository = new ProductCatgoryRepository();
            SaveCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
               
                updateBrand();
            });
        }

        public void LoadData(Brand brand)
        {
            Id= brand.Id;
            Name = brand.Name;
            Logo = brand.Logo;
        }
        public void updateBrand()
        {
            Brand brand = new Brand();
            brand.Name = Name;
            brand.Logo = Logo;
            if (ProductCatgoryRepository.UpdateBrand(brand,Id))
            {
                MessageBox.Show("Cập nhật thành công");
                Application.Current.Windows[1].DialogResult = true;
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại");
            }
        }
    }
}
