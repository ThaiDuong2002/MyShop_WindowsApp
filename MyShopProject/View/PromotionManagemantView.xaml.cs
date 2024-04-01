using MyShopProject.ViewModel;
using System.Windows.Controls;

namespace MyShopProject.View
{
    /// <summary>
    /// Interaction logic for PromotionManagemantView.xaml
    /// </summary>
    public partial class PromotionManagemantView : Page
    {
        private PromotionManagementViewModel _viewModel;
        public PromotionManagemantView()
        {
            InitializeComponent();
            _viewModel = new PromotionManagementViewModel();
            DataContext = _viewModel;

            Global.GetActiveButton();
            Global.SaveScreen("QLKM");
        }
    }
}
