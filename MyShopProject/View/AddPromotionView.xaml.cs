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
    /// Interaction logic for AddPromotionView.xaml
    /// </summary>
    public partial class AddPromotionView : Window
    {
        private AddPromotionViewModel _viewModel;
        public AddPromotionView()
        {
            InitializeComponent();
            _viewModel = new AddPromotionViewModel();
            this.DataContext = _viewModel;
        }
    }
}
