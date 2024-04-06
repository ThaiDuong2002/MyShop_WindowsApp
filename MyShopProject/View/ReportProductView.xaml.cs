using MyShopProject.ViewModel;
using System.Windows.Controls;

namespace MyShopProject.View
{
    /// <summary>
    /// Interaction logic for ReportProductView.xaml
    /// </summary>
    public partial class ReportProductView : Page
    {
        private ReportProductViewModel _viewModel;
        public ReportProductView()
        {
            InitializeComponent();
            _viewModel = new ReportProductViewModel();
            DataContext = _viewModel;

            Global.GetActiveButton();
            Global.SaveScreen("TKBH");
        }
    }
}
