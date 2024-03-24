using MyShopProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void addCustomerButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
