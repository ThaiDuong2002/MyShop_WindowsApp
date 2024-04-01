using MyShopProject.ViewModel;
using System.Windows.Controls;

namespace MyShopProject.View
{
    /// <summary>
    /// Interaction logic for ProductCategoryManagementView.xaml
    /// </summary>
    public partial class ProductCategoryManagementView : Page
    {
        public ProductCategoryManagementViewModel _viewModel;
        public ProductCategoryManagementView()
        {
            InitializeComponent();
            _viewModel = new ProductCategoryManagementViewModel();
            DataContext = _viewModel;
            listView.ItemsSource = _viewModel.Brands;

            Global.GetActiveButton();
            Global.SaveScreen("QLLOAISP");
        }
    }
}
