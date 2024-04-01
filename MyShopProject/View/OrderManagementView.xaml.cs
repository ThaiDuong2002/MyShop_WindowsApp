using MyShopProject.ViewModel;
using System.Windows.Controls;

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
    }
}
