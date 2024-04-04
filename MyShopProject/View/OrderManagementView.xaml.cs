using MyShopProject.Repositories;
using MyShopProject.ViewModel;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MyShopProject.View
{
    /// <summary>
    /// Interaction logic for OrderManagementView.xaml
    /// </summary>
    public partial class OrderManagementView : Page
    {
        private OrderManagementViewModel _viewModel;
        public OrderManagementView()
        {
            InitializeComponent();
            _viewModel = new OrderManagementViewModel();
            DataContext = _viewModel;
            allOrder.ItemsSource = _viewModel.OrderDetails;

            Global.GetActiveButton();
            Global.SaveScreen("QLDH");
        }

        private void ViewOrderDetail_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OrderDetail orderDetail = (OrderDetail)allOrder.SelectedItem;
            OrderDetailView orderDetailView = new OrderDetailView(orderDetail, this);
            NavigationService.Navigate(orderDetailView);
        }
    }
}
