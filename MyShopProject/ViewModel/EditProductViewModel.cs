using MyShopProject.Model;
using MyShopProject.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace MyShopProject.ViewModel
{
    public class EditProductViewModel:BaseViewModel
    {
        public ProductRepository _productRepository;

        private string _title = "Chỉnh sửa sản phẩm";
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
        private string _image;
        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged();
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
        public ICommand SaveCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }
        public EditProductViewModel()
        {
            _productRepository = new ProductRepository();

            SaveCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                updateProduct(p);
            });


            ChooseImageCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChooseImage();
            });
          
        }
        public ProductCatgoryRepository _productCatgoryRepository { get; set; }
        public int Id;
        public void LoadProduct(Product editProduct)
        {
            Id = editProduct.Id;
            Name = editProduct.Name;
            SelectedCategory =editProduct.Brand.Name;
            Price = editProduct.Price;
            Quantity = editProduct.Quantity.Value;
            WarrantyPeriod = editProduct.WarrantyPeriod;
            Weight = (float)editProduct.Weight;
            Image = editProduct.Image;
        }

        public void updateProduct(Window p)
        {
            var BrandId = new ProductCatgoryRepository().GetBrandByName(SelectedCategory).Id;
            var product = new Product()
            {
                Name = Name,
                BrandId = BrandId,
                Price = Price,
                Quantity = Quantity,
                WarrantyPeriod = WarrantyPeriod,
                Weight = Weight,
                Image = Image
            };
            if(_productRepository.UpdateProduct(product,Id))
            {
               System.Windows.MessageBox.Show("Cập nhật sản phẩm thành công");
               p.DialogResult = true;                          
            }
            else
            {
                System.Windows.MessageBox.Show("Cập nhật sản phẩm thất bại");
            }
        }

        public void ChooseImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                Image = new Uri(openFileDialog.FileName).ToString();
                
            }
        }
    }
}
