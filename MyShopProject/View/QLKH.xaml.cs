using MyShopProject.Model;
using MyShopProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        }
    }
}
