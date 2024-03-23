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

        private void editCustomerBtn(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button).DataContext as User;
            ChinhSuaKHView chinhSuaKHView = new ChinhSuaKHView(user.Id);
            if(chinhSuaKHView.ShowDialog() == true)
            {
                _viewModel.LoadData();
            }   
        }

        private void deleteCustomerBtn(object sender, RoutedEventArgs e)
        {
            var user = (sender as Button).DataContext as User;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _viewModel.DeleteUser(user);
            }
        }
    }
}
