using MyShopProject.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MyShopProject.ViewModel
{
    public class OrderManagementViewModel : BaseViewModel
    {
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        private DateTime? _startDate;
        private DateTime? _endDate;
        private int _currentPage = 1;
        private int _totalPage = 1;
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
        private readonly OrderRepository _orderRepository = new OrderRepository();
        private ObservableCollection<OrderDetail> _orderDetails;
        public ObservableCollection<OrderDetail> OrderDetails
        {
            get => _orderDetails;
            set
            {
                _orderDetails = value;
                OnPropertyChanged(nameof(OrderDetails));
            }
        }
        public ICommand FilterOrderCommand { get; set; }
        public ICommand prevBtn { get; set; }
        public ICommand nextBtn { get; set; }
        public OrderManagementViewModel()
        {
            _startDate = null;
            _endDate = null;
            _pageInfo = $"Trang {_currentPage}/{_totalPage}";
            _amountProduct = "0 đơn hàng";
            _orderDetails = new ObservableCollection<OrderDetail>();
            prevBtn = new RelayCommand<object>((p) => { return _currentPage > 1; }, (p) => { Prev(); });
            nextBtn = new RelayCommand<object>((p) => { return _currentPage < _totalPage; }, (p) => { Next(); });
            FilterOrderCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                LoadData();
            });
            LoadData();
        }

        private void LoadData()
        {
            (ObservableCollection<OrderDetail> ordersList, int totalCount) result = _orderRepository.GetAllOrderDetailsByPagination(StartDate, EndDate, _currentPage, SelectedItemsPerPage);
            OrderDetails = result.ordersList;
            int totalProducts = result.totalCount;
            AmountProduct = $"{totalProducts} đơn hàng";
            _totalPage = totalProducts / SelectedItemsPerPage + (totalProducts % SelectedItemsPerPage == 0 ? 0 : 1);
            PageInfo = $"Trang {_currentPage}/{_totalPage}";
        }

        //private void FilterOrder(object p)
        //{
        //    if (StartDate == null || EndDate == null)
        //    {
        //        LoadData();
        //    }

        //    var filteredOrders = new ObservableCollection<OrderDetail>(_orderRepository.GetAllOrderDetailsByPagination(StartDate, EndDate, _currentPage, SelectedItemsPerPage));
        //    if (filteredOrders.Count == 0)
        //    {
        //        _orderDetails.Clear();
        //    }
        //}

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
    }
}
