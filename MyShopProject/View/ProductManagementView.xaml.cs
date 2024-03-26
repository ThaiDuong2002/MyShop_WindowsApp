using MyShopProject.UserControls;
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
    /// Interaction logic for ProductManagementView.xaml
    /// </summary>
    public partial class ProductManagementView : Page
    {
        public ProductManagementViewModel _viewModel;
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
    }
}
