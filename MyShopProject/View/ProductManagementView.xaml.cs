using MyShopProject.Model;
using MyShopProject.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace MyShopProject.View
{
    /// <summary>
    /// Interaction logic for ProductManagementView.xaml
    /// </summary>
    public partial class ProductManagementView : Page
    {
        public ProductManagementViewModel _viewModel;
        public Page currentPage;
        public ProductManagementView()
        {
            InitializeComponent();
            _viewModel = new ProductManagementViewModel();
            DataContext = _viewModel;


            Global.GetActiveButton();
            Global.SaveScreen("QLSP");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchProductInput.Text))
            {
                searchHintTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                searchHintTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                currentPage = this;
                var product = listView.SelectedItem as Product;
                var detailProductPage = new DetailProductView(product, currentPage);
                NavigationService.Navigate(detailProductPage);
            }

        }
    }
}
