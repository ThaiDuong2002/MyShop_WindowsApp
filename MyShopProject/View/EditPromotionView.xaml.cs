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
    /// Interaction logic for EditPromotionView.xaml
    /// </summary>
    public partial class EditPromotionView : Window
    {
        private EditPromotionViewModel _viewModel;
        public EditPromotionView(Promotion editPromotion)
        {
            InitializeComponent();
            _viewModel = new EditPromotionViewModel();
            _viewModel.LoadPromotion(editPromotion);
            this.DataContext = _viewModel;
        }
    }
}
