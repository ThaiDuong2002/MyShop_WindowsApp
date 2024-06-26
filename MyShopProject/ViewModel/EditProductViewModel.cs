﻿using Microsoft.Win32;
using MyShopProject.Model;
using MyShopProject.Repositories;
using System.Windows;
using System.Windows.Input;

namespace MyShopProject.ViewModel
{
    public class EditProductViewModel : BaseViewModel
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
        private int _priceOriginal;
        public int PriceOriginal
        {
            get { return _priceOriginal; }
            set
            {
                _priceOriginal = value;
                OnPropertyChanged(nameof(PriceOriginal));
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
            if (editProduct.Brand != null)
            {
                SelectedCategory = editProduct.Brand.Name;
            }
            Price = editProduct.Price;
            PriceOriginal = editProduct.PriceOriginal;
            Weight = (float)editProduct.Weight;
            Quantity = editProduct.Quantity;
            Image = editProduct.Image;
        }

        public void updateProduct(Window p)
        {
            if (string.IsNullOrEmpty(Name))
            {
                System.Windows.MessageBox.Show("Vui lòng nhập tên sản phẩm");
                return;
            }
            if (string.IsNullOrEmpty(SelectedCategory))
            {
                System.Windows.MessageBox.Show("Vui lòng chọn loại sản phẩm");
                return;
            }
            if (Price == 0)
            {
                System.Windows.MessageBox.Show("Vui lòng nhập giá bán sản phẩm");
                return;
            }
            if (PriceOriginal == 0)
            {
                System.Windows.MessageBox.Show("Vui lòng nhập giá gốc sản phẩm");
                return;
            }
            if (Quantity == 0)
            {
                System.Windows.MessageBox.Show("Vui lòng nhập số lượng sản phẩm");
                return;
            }
            if (Weight == 0)
            {
                System.Windows.MessageBox.Show("Vui lòng nhập trọng lượng sản phẩm");
                return;
            }
            if (string.IsNullOrEmpty(Image))
            {
                System.Windows.MessageBox.Show("Vui lòng chọn ảnh sản phẩm");
                return;
            }
            var BrandId = new ProductCatgoryRepository().GetBrandByName(SelectedCategory).Id;
            var product = new Product()
            {
                Name = Name,
                BrandId = BrandId,
                Price = Price,
                PriceOriginal = PriceOriginal,
                Quantity = Quantity,
                Weight = Weight,
                Image = Image
            };
            if (_productRepository.UpdateProduct(product, Id))
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
