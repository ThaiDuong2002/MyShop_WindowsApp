using MyShopProject.ViewModel;
using System.Windows.Controls;

namespace MyShopProject.View
{
    /// <summary>
    /// Interaction logic for RevenueStatisticsView.xaml
    /// </summary>
    public partial class RevenueStatisticsView : Page
    {
        private RevenueStatisticsViewModel _viewModel;
        public RevenueStatisticsView()
        {
            InitializeComponent();
            _viewModel = new RevenueStatisticsViewModel();
            DataContext = _viewModel;

            Global.GetActiveButton();
            Global.SaveScreen("TKDTVLN");
        }
    }
}
