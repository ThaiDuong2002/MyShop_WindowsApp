using MyShopProject.ViewModel;
using System.Windows.Controls;

namespace MyShopProject.View
{
    /// <summary>
    /// Interaction logic for ProductStatisticsView.xaml
    /// </summary>
    public partial class ProductStatisticsView : Page
    {
        private ProductStatisticsViewModel _viewModel;
        public ProductStatisticsView()
        {
            InitializeComponent();
            _viewModel = new ProductStatisticsViewModel();
            DataContext = _viewModel;

            Global.GetActiveButton();
            Global.SaveScreen("TKSP");
        }
    }
}
