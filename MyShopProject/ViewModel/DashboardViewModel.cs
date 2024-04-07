using MyShopProject.Model;
using MyShopProject.Repositories;
using System.Collections.ObjectModel;

namespace MyShopProject.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        private int _quantityProductAvailable = 0;
        public int quantityProductAvailable
        {
            get { return _quantityProductAvailable; }
            set
            {
                _quantityProductAvailable = value;
                OnPropertyChanged(nameof(quantityProductAvailable));
            }
        }
        private int _quantityNewPurchaseInWeek = 0;
        public int quantityNewPurchaseInWeek
        {
            get { return _quantityNewPurchaseInWeek; }
            set
            {
                _quantityNewPurchaseInWeek = value;
                OnPropertyChanged(nameof(quantityNewPurchaseInWeek));
            }
        }
        private int _quantityNewPurchaseInMonth = 0;
        public int quantityNewPurchaseInMonth
        {
            get { return _quantityNewPurchaseInMonth; }
            set
            {
                _quantityNewPurchaseInMonth = value;
                OnPropertyChanged(nameof(quantityNewPurchaseInMonth));
            }
        }
        public ObservableCollection<Product> _top5ProductSoldList;
        public ObservableCollection<Product> top5ProductSoldList
        {
            get { return _top5ProductSoldList; }
            set
            {
                _top5ProductSoldList = value;
                OnPropertyChanged(nameof(top5ProductSoldList));
            }
        }
        public ProductRepository _productRepository = new ProductRepository();
        public OrderRepository _orderRepository = new OrderRepository();
        public DashboardViewModel()
        {
            quantityProductAvailable = _productRepository.getNumOfProductsAvailable();
            quantityNewPurchaseInWeek = _orderRepository.getNumOfNewPurchaseInWeek(DateTime.Now);
            quantityNewPurchaseInMonth = _orderRepository.getNumOfNewPurchaseInMonth(DateTime.Now);
            top5ProductSoldList = _productRepository.getTop5Product();


            Global.SaveScreen("Dashboard");
        }
    }
}
