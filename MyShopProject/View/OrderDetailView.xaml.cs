using MyShopProject.Model;
using MyShopProject.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyShopProject.View
{
    /// <summary>
    /// Interaction logic for OrderDetailView.xaml
    /// </summary>
    public partial class OrderDetailView : Page
    {
        private Page _prePage;
        private OrderDetail _orderDetail;
        private OrderRepository _orderRepository = new OrderRepository();
        private OrderProductRepository _orderProductRepository = new OrderProductRepository();
        public OrderDetailView(OrderDetail orderDetail, Page page)
        {
            InitializeComponent();
            _prePage = page;
            _orderDetail = orderDetail;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(_prePage);
        }
        private class CombinedOrder
        {
            public User User { get; set; }
            public ObservableCollection<OrderProduct> OrderProducts { get; set; }
            public int TotalPrice { get; set; }
        }
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Order order = _orderRepository.GetOrderById(_orderDetail.Id);
            (Order? orderDetail, int numOfProduct, int numOfProductType) = _orderRepository.GetNumberOfOrderProductByOrderId(_orderDetail.Id);
            //UserName.Text = order.User.Name;
            //Birthday.Text = order.User.Birthday.ToString("dd/MM/yyyy");
            //Address.Text = order.User.Address;
            //Phone.Text = order.User.Phone;

            CreatedAt.Text = orderDetail!.CreatedAt.ToString("dd/MM/yyyy");
            NumOfProduct.Text = numOfProduct.ToString();
            NumOfProductType.Text = numOfProductType.ToString();

            ObservableCollection<OrderProduct> orderProduct = _orderProductRepository.GetOrderProductsByOrderId(_orderDetail.Id);
            int totalPrice = _orderProductRepository.GetTotalPriceByOrderId(_orderDetail.Id);
            CombinedOrder combinedOrder = new CombinedOrder();
            combinedOrder.User = order.User;
            combinedOrder.OrderProducts = orderProduct;
            combinedOrder.TotalPrice = totalPrice;
            DataContext = combinedOrder;
        }
    }
}
