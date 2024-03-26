using Microsoft.Win32;
using MyShopProject.Interface;
using MyShopProject.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MyShopProject.ViewModel
{
    public class AddProductViewModel :BaseViewModel
    {
        public ProductRepository _productRepository;

        private string _title="Thêm sản phẩm";
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _selectedcategory;
        public string SelectedCategory
        {
            get { return _selectedcategory; }
            set
            {
                _selectedcategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }
       
        private int _price;
        public int Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        private string _warrantyPeriod;
        public string WarrantyPeriod
        {
            get { return _warrantyPeriod; }
            set
            {
                _warrantyPeriod = value;
                OnPropertyChanged(nameof(WarrantyPeriod));
            }
        }
        private float _weight;
        public float Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }
        private BitmapImage _image;
        public BitmapImage Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        private List<string> _categories = new ProductCatgoryRepository().GetAllBrandsName();
        public List<string> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
        public ICommand ChooseImageCommand { get; set; }
        public AddProductViewModel()
        {
            _productRepository = new ProductRepository();
            ChooseImageCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ChooseImage();
            });
            AddCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                AddProduct(p);
            });

        }
        public ICommand AddCommand { get; set; }
        private string imageProduct;
        public void AddProduct(Window p)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(SelectedCategory)  || string.IsNullOrEmpty(WarrantyPeriod))
            {
                return;
            }
            var product = new Model.Product()
            {
                Name = Name,
                BrandId = new ProductCatgoryRepository().GetBrandByName(SelectedCategory).Id,
                Price = Price,
                Quantity = Quantity,
                WarrantyPeriod = WarrantyPeriod,
                Weight = Weight,
                Image = imageProduct
            };
            if(_productRepository.AddProduct(product))
            {
                System.Windows.MessageBox.Show("Thêm sản phẩm thành công");
                p.DialogResult = true;
             
            }
            else
            {
                System.Windows.MessageBox.Show("Thêm sản phẩm thất bại");
            }
        }

        public void ChooseImage()
        {
           OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
               Image = new BitmapImage(new Uri(openFileDialog.FileName));
               imageProduct = new Uri(openFileDialog.FileName).ToString();
            }
        }
    }
}
