using Microsoft.Win32;
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
        public ICommand SaveCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }
        public EditBrandViewModel()
        {
            ProductCatgoryRepository = new ProductCatgoryRepository();
            SaveCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {

                updateBrand(p);
            });

            ChooseImageCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChooseImage();
            });


        }

        public void LoadData(Brand brand)
        {
            Id = brand.Id;
            Name = brand.Name;
            Logo = brand.Logo;
        }
        public void updateBrand(Window p)
        {
            if (string.IsNullOrEmpty(Name))
            {
                System.Windows.MessageBox.Show("Vui lòng nhập tên thương hiệu");
                return;
            }
            Brand brand = new Brand();
            brand.Name = Name;
            brand.Logo = Logo;
            if (ProductCatgoryRepository.UpdateBrand(brand, Id))
            {
                System.Windows.MessageBox.Show("Cập nhật thành công");
                p.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại");
            }
        }

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
