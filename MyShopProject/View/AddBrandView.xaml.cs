using MyShopProject.Model;
using MyShopProject.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace MyShopProject.View
{
    /// <summary>
    /// Interaction logic for AddBrandView.xaml
    /// </summary>
    public partial class AddBrandView : Window
    {
        public AddBrandViewModel _viewModel;
        public AddBrandView()
        {
            InitializeComponent();
            _viewModel = new AddBrandViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var brand = new Brand()
            {
                Name = addBrand.brandName.Text,
                Logo = File.ReadAllBytes(addBrand.fileName.Text)
            };
            _viewModel.AddBrand(brand);
            addBrand.brandName.Text = "";


        }
    }
}
