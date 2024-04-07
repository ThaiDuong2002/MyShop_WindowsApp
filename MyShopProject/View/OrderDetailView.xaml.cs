using MyShopProject.Model;
using MyShopProject.Repositories;
using System.Collections.ObjectModel;
using System.Windows;
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
        private ProductRepository _productRepository = new ProductRepository();
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
            if (_orderDetail.Status == 0)
            {
                OrderStatusBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                OrderStatusText.Text = "Đã hủy";
                OrderStatusText.Foreground = System.Windows.Media.Brushes.Red;
            }
            else if (_orderDetail.Status == 1)
            {
                OrderStatusBorder.BorderBrush = System.Windows.Media.Brushes.Blue;
                OrderStatusText.Text = "Đang xử lý";
                OrderStatusText.Foreground = System.Windows.Media.Brushes.Blue;
            }
            else
            {
                OrderStatusBorder.BorderBrush = System.Windows.Media.Brushes.Green;
                OrderStatusText.Text = "Đã hoàn thành";
                OrderStatusText.Foreground = System.Windows.Media.Brushes.Green;
            }

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
            if (_orderDetail.Status != 1)
            {
                AcceptedBtn.IsEnabled = false;
                CancelBtn.IsEnabled = false;
            }
        }

        private void AcceptedBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn xác nhận hoàn thành đơn?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _orderRepository.UpdateOrderByStatus(_orderDetail.Id, 2);
                AcceptedBtn.IsEnabled = false;
                CancelBtn.IsEnabled = false;
                OrderStatusBorder.BorderBrush = System.Windows.Media.Brushes.Green;
                OrderStatusText.Text = "Đã hoàn thành";
                OrderStatusText.Foreground = System.Windows.Media.Brushes.Green;
                ObservableCollection<OrderProduct> orderProducts = _orderProductRepository.GetOrderProductsByOrderId(_orderDetail.Id);
                foreach (OrderProduct product in orderProducts)
                {
                    bool IsAvailable = _productRepository.CheckProductQuantity(product.ProductId, product.Amount);
                    if (!IsAvailable)
                    {
                        MessageBox.Show("Sản phẩm " + product.Product.Name + " không đủ số lượng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                foreach (var orderProduct in orderProducts)
                {
                    _productRepository.UpdateQuantityAfterOrder(orderProduct.ProductId, orderProduct.Amount);
                }
                _orderRepository.UpdateEditOrderDate(_orderDetail.Id, DateTime.Now);
            }
            else
            {
                return;
            }
        }

        private void CancelBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn xác nhận hủy đơn?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _orderRepository.UpdateOrderByStatus(_orderDetail.Id, 0);
                AcceptedBtn.IsEnabled = false;
                CancelBtn.IsEnabled = false;
                OrderStatusBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                OrderStatusText.Text = "Đã hủy";
                OrderStatusText.Foreground = System.Windows.Media.Brushes.Red;
                _orderRepository.UpdateEditOrderDate(_orderDetail.Id, DateTime.Now);
            }
            else
            {
                return;
            }
        }
    }
}
