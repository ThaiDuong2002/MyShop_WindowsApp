using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using MyShopProject.Model;
using MyShopProject.Repositories;
using MyShopProject.View;
using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MyShopProject.ViewModel
{
    public class ProductCategoryManagementViewModel : BaseViewModel
    {
        private string _searchBrandText;
        public string SearchBrandText
        {
            get { return _searchBrandText; }
            set
            {
                if (_searchBrandText != value) // Kiểm tra xem giá trị đã thay đổi hay không
                {
                    _searchBrandText = value;
                    OnPropertyChanged(nameof(SearchBrandText));

                    if (string.IsNullOrEmpty(_searchBrandText)) // Kiểm tra xem SearchText có bị xóa không
                    {
                        LoadData(); // Nếu đã xóa, tải lại dữ liệu ban đầu
                    }
                }
            }
        }

        public ProductCatgoryRepository _productCatgoryRepository;
        private ObservableCollection<Brand> _brands;
        public ObservableCollection<Brand> Brands
        {
            get => _brands;
            set
            {
                _brands = value;
                OnPropertyChanged();
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

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand prevBtn { get; set; }
        public ICommand nextBtn { get; set; }
        public ICommand searchBtn { get; set; }
        public ICommand ImportBrandCommand { get; set; }
        public ProductCategoryManagementViewModel()
        {
            _productCatgoryRepository = new ProductCatgoryRepository();
            AddCommand = new RelayCommand<object>((p) => { return true; }, (p) => { AddBrandWindow(); });
            EditCommand = new RelayCommand<Brand>((p) => { return true; }, (p) => { EditBrandWindow(p); });
            DeleteCommand = new RelayCommand<Brand>((p) => { return true; }, (p) => { DeleteBrand(p); });
            searchBtn = new RelayCommand<object>((p) => { return !string.IsNullOrEmpty(SearchBrandText); }, (p) => { Search(); });
            prevBtn = new RelayCommand<object>((p) => { return _currentPage > 1; }, (p) => { Prev(); });
            nextBtn = new RelayCommand<object>((p) => { return _currentPage < _totalPage; }, (p) => { Next(); });
            ImportBrandCommand = new RelayCommand<object>((p) => { return true; }, (p) => { ImportBrand(); });
            LoadData();

        }
        public int _currentPage = 1;
        public int _totalPage = 1;
        public int _itemPerPage = 8;
        public void LoadData()
        {
            if (!Brands.IsNullOrEmpty() || _totalPage == 0)
            {
                Brands.Clear();
                var brands = _productCatgoryRepository.GetBrandByPagination(_currentPage, _itemPerPage);
                if (brands.IsNullOrEmpty())
                {
                    _currentPage--;
                }
                foreach (var item in _productCatgoryRepository.GetBrandByPagination(_currentPage, _itemPerPage))
                {
                    Brands.Add(item);
                }

            }
            else
            {
                var brands = _productCatgoryRepository.GetBrandByPagination(_currentPage, _itemPerPage);
                Brands = new ObservableCollection<Brand>(brands);
            }
            var totalBrands = _productCatgoryRepository.GetNumOfBrands();
            _totalPage = (totalBrands % _itemPerPage == 0) ? totalBrands / _itemPerPage : totalBrands / _itemPerPage + 1;
            PageInfo = $"Trang {_currentPage}/{_totalPage}";
            AmountProduct = $"Tổng số thương hiệu: {totalBrands}";
        }
 

        public void AddBrandWindow()
        {

            AddBrandView addBrandView = new AddBrandView();
            addBrandView.ShowDialog();
            if(addBrandView.DialogResult == true)
            {
                LoadData();
            }
        }

        public void EditBrandWindow(Brand SelectedBrand)
        {
            EditBrandView editBrandView = new EditBrandView(SelectedBrand);
            if (editBrandView.ShowDialog() == true)
            {
                int index = Brands.IndexOf(SelectedBrand);
                Brands[index] = _productCatgoryRepository.GetBrandById(SelectedBrand.Id);
            }
        }

        public void DeleteBrand(Brand SelectedBrand)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa thương hiệu này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (_productCatgoryRepository.DeleteBrand(SelectedBrand.Id))
                {
                    MessageBox.Show("Xóa thương hiệu thành công");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Xóa thương hiệu thất bại");
                }
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
            Brands.Clear();
            foreach (var brand in _productCatgoryRepository.SearchBrandByName(SearchBrandText))
            {
                Brands.Add(brand);
            }
            int numOfBrands = Brands.Count;// Lấy số lượng người dùng
            _totalPage = numOfBrands / _itemPerPage + (numOfBrands % _itemPerPage == 0 ? 0 : 1); // Tính toán số trang
            PageInfo = $"Trang {_currentPage}/{_totalPage}";
        }
        public void ImportBrand()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                var package = new ExcelPackage(new FileInfo(path));
                var workSheet = package.Workbook.Worksheets[1];
                int totalRows = workSheet.Dimension.Rows;
                try
                {
                    for(int i = 2; i <= totalRows; i++)
                    {
                        var brand = new Brand()
                        {
                            Name = workSheet.Cells[i, 1].Value.ToString(),
                            Logo = workSheet.Cells[i, 2].Value.ToString()
                        };
                        _productCatgoryRepository.AddBrand(brand);
                    }
                    MessageBox.Show("Import thương hiệu thành công");
                    LoadData();
                }
                catch
                {
                    MessageBox.Show("Import thương hiệu thất bại");
                }
            }
        }
    }
}
