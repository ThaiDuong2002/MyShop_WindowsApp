using MyShopProject.Model;
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
using System.Windows.Shapes;

namespace MyShopProject.View
{
    /// <summary>
    /// Interaction logic for ChinhSuaKHView.xaml
    /// </summary>
    public partial class ChinhSuaKHView : Window
    {
        public ChinhSuaKHViewModel _viewModel;
        public ChinhSuaKHView(int id)
        {
            InitializeComponent();
            _viewModel = new ChinhSuaKHViewModel();
            _viewModel.Load(id);
             DataContext = _viewModel;
        }
    }
}
