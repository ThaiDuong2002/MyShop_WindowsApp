using MyShopProject.Model;
using MyShopProject.Repositories;
using MyShopProject.Utilities;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MyShopProject.ViewModel
{
    public class AddOrderViewModel : BaseViewModel
    {
        private ProductRepository _productRepository = new ProductRepository();
        private UserRepository _userRepository = new UserRepository();
        private PromotionRepository _promotionRepository = new PromotionRepository();
        private OrderRepository _orderRepository = new OrderRepository();
        private OrderProductRepository _orderProductRepository = new OrderProductRepository();
        private int _currentPage = 1;
        private int _totalPage = 1;
        public List<int> LongIntegerList = new List<int>(Enumerable.Range(0, 1000));
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }
        private ObservableCollection<OrderProductStore> _orderProductStores;
        public ObservableCollection<OrderProductStore> OrderProductStores
        {
            get => _orderProductStores;
            set
            {
                _orderProductStores = value;
                OnPropertyChanged(nameof(OrderProductStores));
            }
        }
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Promotion> _promotions;
        public ObservableCollection<Promotion> Promotions
        {
            get => _promotions;
            set
            {
                _promotions = value;
                OnPropertyChanged();
            }
        }
        private string _searchProductText;
        public string SearchProductText
        {
            get { return _searchProductText; }
            set
            {
                if (_searchProductText != value)
                {
                    _searchProductText = value;
                    OnPropertyChanged(nameof(SearchProductText));

                    if (string.IsNullOrEmpty(_searchProductText))
                    {
                        LoadData();
                    }
                }
            }
        }
        public List<int> ItemsPerPage => new List<int> { 5, 10, 15, 20 };
        private int _selectedItemsPerPage = 5;
        public int SelectedItemsPerPage
        {
            get => _selectedItemsPerPage;
            set
            {
                _selectedItemsPerPage = value;
                OnPropertyChanged();
                LoadData();
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
        private int _totalPrice;
        public int TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
        public User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        public ICommand prevBtn { get; set; }
        public ICommand nextBtn { get; set; }
        public ICommand KeyDownCommand { get; set; }
        public ICommand AddOrderCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand RemoveProductCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public AddOrderViewModel()
        {
            OrderProductStorage.ClearOrderProductStore();
            _products = new ObservableCollection<Product>();
            _orderProductStores = new ObservableCollection<OrderProductStore>();
            _promotions = new ObservableCollection<Promotion>();
            _selectedUser = new User();
            _users = new ObservableCollection<User>();
            _pageInfo = $"Trang {_currentPage}/{_totalPage}";
            _searchProductText = "";
            prevBtn = new RelayCommand<object>((p) => { return _currentPage > 1; }, (p) => { Prev(); });
            nextBtn = new RelayCommand<object>((p) => { return _currentPage < _totalPage; }, (p) => { Next(); });
            KeyDownCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadData(); });
            AddProductCommand = new RelayCommand<Product>((p) => { return p != null; }, (p) => { AddProduct(p); });
            AddOrderCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { AddOrder(p); });
            RemoveProductCommand = new RelayCommand<OrderProductStore>((p) => { return p != null; }, (p) => { RemoveProduct(p); });
            CancelCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
            LoadData();
        }

        private void LoadData()
        {
            (int totalCount, ObservableCollection<Product> products) = _productRepository.GetFilteredProducts(0, _currentPage, SelectedItemsPerPage, null, SearchProductText, null, null);
            Users = _userRepository.GetAllUsers();
            Promotions = _promotionRepository.GetAllPromotions();
            OrderProductStores = OrderProductStorage.OrderProductStores;
            Products = products;
            int totalProducts = totalCount;
            _totalPage = totalProducts / SelectedItemsPerPage + (totalProducts % SelectedItemsPerPage == 0 ? 0 : 1);
            PageInfo = $"Trang {_currentPage}/{_totalPage}";
        }

        private void Prev()
        {
            _currentPage--;
            LoadData();
        }

        private void Next()
        {
            _currentPage++;
            LoadData();
        }
        private void AddProduct(Product p)
        {
            if (p.Quantity == 0)
            {
                MessageBox.Show("Sản phẩm đã hết hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            OrderProductStorage.AddOrderProductStore(new OrderProductStore
            {
                ProductId = p.Id,
                ProductName = p.Name,
                Price = p.Price,
                Quantity = 1,
            });
            TotalPrice = OrderProductStorage.TotalPrice();
        }
        private void RemoveProduct(OrderProductStore p)
        {
            OrderProductStorage.RemoveOrderProductStore(p);
            TotalPrice = OrderProductStorage.TotalPrice();
        }
        private void AddOrder(Window p)
        {
            if (OrderProductStorage.OrderProductStores.Count == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn sản phẩm!",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );
                return;
            }
            if (SelectedUser.Id == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int id = _orderRepository.AddOrder(new Order
            {
                UserId = SelectedUser.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = 1,
            });
            foreach (var orderProductStore in OrderProductStorage.OrderProductStores)
            {
                bool IsAvailable = _productRepository.CheckProductQuantity(orderProductStore.ProductId, orderProductStore.Quantity);
                if (!IsAvailable)
                {
                    MessageBox.Show("Sản phẩm " + orderProductStore.ProductName + " không đủ số lượng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            foreach (var orderProductStore in OrderProductStorage.OrderProductStores)
            {
                OrderProduct orderProduct = new OrderProduct
                {
                    OrderId = id,
                    ProductId = orderProductStore.ProductId,
                    Amount = orderProductStore.Quantity,
                    PromotionId = orderProductStore.Promotion?.Id,
                };
                _orderProductRepository.AddOrderProduct(orderProduct);
            }
            MessageBox.Show("Thêm đơn hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            OrderProductStorage.ClearOrderProductStore();
            p.DialogResult = true;
            p.Close();
        }
    }
}
