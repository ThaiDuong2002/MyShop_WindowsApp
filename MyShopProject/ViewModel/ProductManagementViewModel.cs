using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using MyShopProject.Model;
using MyShopProject.Repositories;
using MyShopProject.View;
using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MyShopProject.ViewModel
{
    public class ProductManagementViewModel : BaseViewModel
    {
        public ProductRepository _productRepository;
        public ProductCatgoryRepository _productCatgoryRepository;
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }



        private string _searchProductText;
        public string SearchProductText
        {
            get { return _searchProductText; }
            set
            {
                if (_searchProductText != value) // Kiểm tra xem giá trị đã thay đổi hay không
                {
                    _searchProductText = value;
                    OnPropertyChanged(nameof(SearchProductText));

                    if (string.IsNullOrEmpty(_searchProductText)) // Kiểm tra xem SearchText có bị xóa không
                    {
                        LoadData(); // Nếu đã xóa, tải lại dữ liệu ban đầu
                    }
                }
            }
        }

        private List<string> _categories;
        public List<string> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        private string _pageInfo;
        public string PageInfo
        {
            get { return _pageInfo; }
            set
            {
                _pageInfo = value;
                OnPropertyChanged(nameof(PageInfo));
            }
        }
        private string _amountProduct;
        public string AmountProduct { get => _amountProduct; set { _amountProduct = value; OnPropertyChanged(); } }
        private int _selectedItemPerPage = 8;
        public int SelectedItemPerPage
        {
            get => _selectedItemPerPage;
            set
            {
                _selectedItemPerPage = value;
                OnPropertyChanged();
                LoadData();
            }
        }
        private int category = 0;
        private string _selectedCategory = "Tất cả";
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                if (value == "Tất cả")
                {
                    category = 0;
                    LoadData();
                    return;
                }
                var brand = _productCatgoryRepository.GetBrandByName(value);
                category = brand.Id;
                LoadData();
            }
        }
        public List<int> ItemsPerPage => new List<int> { 4, 8, 16, 20, 25 };
        private int _currentPage = 1;
        private int _totalPage = 1;

        public ICommand prevBtn { get; set; }
        public ICommand nextBtn { get; set; }
        public ICommand KeyDownCommand { get; set; }
        public ICommand OrderCommand { get; set; }
        public ICommand FilterPriceCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand EditProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand ImportProductCommand { get; set; }



        public ProductManagementViewModel()
        {
            _productRepository = new ProductRepository();
            _productCatgoryRepository = new ProductCatgoryRepository();
            Categories = new List<string>();
            Categories.Add("Tất cả");
            foreach (var item in new ObservableCollection<Brand>(_productCatgoryRepository.GetAllBrands())
)
            {
                Categories.Add(item.Name);
            }

            MaxPrice = _productRepository.getMaxPrice();
            (int totalCount, ObservableCollection<Product> products) result = _productRepository.GetFilteredProducts(category, _currentPage, SelectedItemPerPage, null, null, MinPrice, MaxPrice);
            int totalProducts = result.totalCount;
            Products = result.products;
            prevBtn = new RelayCommand<object>((p) => { return _currentPage > 1; }, (p) => { Prev(); });
            nextBtn = new RelayCommand<object>((p) => { return _currentPage < _totalPage; }, (p) => { Next(); });
            KeyDownCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Search();

            });


            OrderCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                OrderFilterProduct(p);
            });

            FilterPriceCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                LoadData();
            });

            AddProductCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                AddProductWindow();
            });

            EditProductCommand = new RelayCommand<Product>((p) => { return p != null; }, (p) =>
            {
                EditProductWindow(p);
            });

            DeleteProductCommand = new RelayCommand<Product>((p) => { return p != null; }, (p) =>
            {
                DeleteProduct(p);
            });

            ImportProductCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ImportProduct();
            });

            AmountProduct = totalProducts + " sản phẩm";
            _totalPage = totalProducts / SelectedItemPerPage + (totalProducts % SelectedItemPerPage == 0 ? 0 : 1); // Tính toán số trang
            PageInfo = $"Trang {_currentPage}/{_totalPage}";
        }

        public int OrderFilter = -1;
        public void LoadData()
        {
            Products.Clear(); // Xóa danh sách  cũ
            (int totalCount, ObservableCollection<Product> products) result = _productRepository.GetFilteredProducts(category, _currentPage, SelectedItemPerPage, OrderFilter, SearchProductText, MinPrice, MaxPrice);
            int totalProducts = result.totalCount;
            ObservableCollection<Product> product = result.products;
            AmountProduct = totalProducts + " sản phẩm";
            _totalPage = totalProducts / SelectedItemPerPage + (totalProducts % SelectedItemPerPage == 0 ? 0 : 1); // Tính toán số trang
            PageInfo = $"Trang {_currentPage}/{_totalPage}";
            if (product.IsNullOrEmpty())
            {
                if (_currentPage > 1)
                {
                    _currentPage--;
                }
                else
                {
                    return;
                }
            }
            foreach (var item in product)
            {
                Products.Add(item); // Thêm mới vào danh sách
            }
        }

        private bool _isAscendingActive;
        public bool IsAscendingActive
        {
            get => _isAscendingActive;
            set
            {
                _isAscendingActive = value;
                OnPropertyChanged();
            }
        }
        private bool _isDescendingActive;
        public bool IsDescendingActive
        {
            get => _isDescendingActive;
            set
            {
                _isDescendingActive = value;
                OnPropertyChanged();
            }
        }
        private bool _isSaleActive;
        public bool IsSaleActive
        {
            get => _isSaleActive;
            set
            {
                _isSaleActive = value;
                OnPropertyChanged();
            }
        }
        public void OrderFilterProduct(object p)
        {
            if (p.ToString() == "ascending")
            {
                OrderFilter = 0;
                IsAscendingActive = true;
                IsDescendingActive = false;
                IsSaleActive = false;
            }
            else
            {
                OrderFilter = 1;
                IsAscendingActive = false;
                IsDescendingActive = true;
                IsSaleActive = false;
            }
            LoadData();
        }


        private double _minPrice = 0;
        public double MinPrice
        {
            get => _minPrice;
            set
            {
                _minPrice = value;
                OnPropertyChanged();
            }
        }
        private double _maxPrice;
        public double MaxPrice
        {
            get => _maxPrice;
            set
            {
                _maxPrice = value;
                OnPropertyChanged();
            }
        }

        public void Prev()
        {
            _currentPage--;
            LoadData();
        }

        public void Next()
        {
            _currentPage++;
            LoadData();
        }

        public void Search()
        {
            _currentPage = 1;
            Products.Clear();
            (int totalCount, ObservableCollection<Product> products) result = _productRepository.GetFilteredProducts(category, _currentPage, SelectedItemPerPage, OrderFilter, SearchProductText, MinPrice, MaxPrice);

            ObservableCollection<Product> products = result.products;
            foreach (var item in products)
            {
                Products.Add(item);
            }
            int numOfProducts = result.totalCount;
            _totalPage = numOfProducts / 10 + (numOfProducts % 10 == 0 ? 0 : 1); // Tính toán số trang
            PageInfo = $"Trang {_currentPage}/{_totalPage}";
        }

        public void AddProductWindow()
        {
            AddProductView addProductView = new AddProductView();
            addProductView.ShowDialog();
            if (addProductView.DialogResult == true)
            {
                LoadData();
            }

        }
        public void EditProductWindow(Product product)
        {
            EditProductView editProductView = new EditProductView(product);
            editProductView.ShowDialog();
            if (editProductView.DialogResult == true)
            {
                int index = Products.IndexOf(product);
                Products[index] = _productRepository.getProductByID(product.Id);
            }


        }
        public void DeleteProduct(Product p)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (_productRepository.DeleteProduct(p.Id))
                {
                    MessageBox.Show("Xóa sản phẩm thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    Products.Remove(p);
                }
                else
                {
                    MessageBox.Show("Xóa sản phẩm thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void ImportProduct()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                var package = new ExcelPackage(new FileInfo(path));
                var workSheet = package.Workbook.Worksheets[0];
                int totalRows = workSheet.Dimension.Rows;
                try
                {

                    for (int i = 2; i <= totalRows; i++)
                    {
                        Product product = new Product()
                        {
                            Name = workSheet.Cells[i, 1].Value.ToString(),
                            BrandId = int.Parse(workSheet.Cells[i, 2].Value.ToString()),
                            Price = int.Parse(workSheet.Cells[i, 3].Value.ToString()),
                            Quantity = int.Parse(workSheet.Cells[i, 4].Value.ToString()),
                            WarrantyPeriod = workSheet.Cells[i, 5].Value.ToString(),
                            Weight = Math.Round(float.Parse(workSheet.Cells[i, 6].Value.ToString()), 1),
                            Image = workSheet.Cells[i, 7].Value.ToString()
                        };
                        _productRepository.AddProduct(product);
                    }
                    System.Windows.MessageBox.Show("Nhập sản phẩm thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData();
                }
                catch
                {
                    System.Windows.MessageBox.Show("Nhập sản phẩm thất bại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

    }
}
