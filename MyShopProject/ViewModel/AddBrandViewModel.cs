using Microsoft.Win32;
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
    public class AddBrandViewModel : BaseViewModel
    {

        public ICommand AddCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }
        public ProductCatgoryRepository _productCatgoryRepository = new ProductCatgoryRepository();
        public AddBrandViewModel()
        {
            AddCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                AddBrand(p);
            });

            ChooseImageCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ChooseImage();
            });
            

        }

        public void AddBrand(Window p)
        {
            if(string.IsNullOrEmpty(Name))
            {
                System.Windows.MessageBox.Show("Vui lòng nhập tên thương hiệu");
                return;
            }
            Brand brand = new Brand()
            {
                Name = Name,
                Logo = Logo
            };
            if (_productCatgoryRepository.AddBrand(brand))
            {
                System.Windows.MessageBox.Show("Thêm thương hiệu thành công");
                p.DialogResult = true;
            }
            else
            {
                System.Windows.MessageBox.Show("Thêm thương hiệu thất bại");
            }
        }

        private string _name;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }

        private string _logo;
        public string Logo { get => _logo; set { _logo = value; OnPropertyChanged(); } }

        public void ChooseImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
               Logo = new Uri(openFileDialog.FileName).ToString();
            }
        }
    }
}
