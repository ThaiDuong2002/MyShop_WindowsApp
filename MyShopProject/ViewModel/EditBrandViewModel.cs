using MyShopProject.Model;
using MyShopProject.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private string _logo;
        public string Logo
        {
            get { return _logo; }
            set
            {
                _logo = value;
                OnPropertyChanged();
            }
        }
        public int Id;
        public TextBlock fileName { get; set; }
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

        public void LoadData(Brand brand, TextBlock fileName)
        {
            Id = brand.Id;
            Name = brand.Name;
            Logo = brand.Logo;
            this.fileName = fileName;
        }
        public void updateBrand()
        {

            var path = new Uri(this.fileName.Text).ToString();
            Brand brand = new Brand();
            brand.Name = Name;
            brand.Logo = path;
            if (ProductCatgoryRepository.UpdateBrand(brand, Id))
            {
                System.Windows.MessageBox.Show("Cập nhật thành công");
                Application.Current.Windows[1].DialogResult = true;
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại");
            }
        }

        

    }
}
