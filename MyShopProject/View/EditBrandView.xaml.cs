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
    /// Interaction logic for EditBrandView.xaml
    /// </summary>
    public partial class EditBrandView : Window
    {
        public Brand brand { get; set; }
        public EditBrandViewModel _viewmodel;
        public EditBrandView(Brand editBrand)
        {
            InitializeComponent();
            _viewmodel = new EditBrandViewModel();

            _viewmodel.LoadData(editBrand);
            DataContext = _viewmodel;
        }
    }
}
