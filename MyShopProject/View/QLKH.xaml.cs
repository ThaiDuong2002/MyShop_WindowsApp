using MyShopProject.UserControls;
using MyShopProject.ViewModel;
using System.Windows.Controls;

namespace MyShopProject.View
{
    /// <summary>
    /// Interaction logic for QLKH.xaml
    /// </summary>
    public partial class QLKH : Page
    {
        private QLKHViewModel _viewModel;
        public QLKH()
        {
            InitializeComponent();
            _viewModel = new QLKHViewModel();
            DataContext = _viewModel;
            allUser.ItemsSource = _viewModel.Users;

            Global.GetActiveButton();
            Global.SaveScreen("QLKH");
        }
    }
}
