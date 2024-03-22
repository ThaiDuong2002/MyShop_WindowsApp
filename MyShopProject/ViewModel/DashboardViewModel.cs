using MyShopProject.Model;
using MyShopProject.repositories;
using System.Collections.ObjectModel;

namespace MyShopProject.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        public int _quantityProductAvailable = 0;
        public int _quantityNewPurchaseInWeek = 0;
        public int _quantityNewPurchaseInMonth = 0;
        public ObservableCollection<Product> _top5ProductSoldList;
        public ProductRepository _productRepository = new ProductRepository();

        public DashboardViewModel()
        {
            _quantityProductAvailable = _productRepository.getNumOfProductsAvailable();
            _top5ProductSoldList = _productRepository.getTop5Product();

            Global.SaveScreen("Dashboard");
        }
    }
}
