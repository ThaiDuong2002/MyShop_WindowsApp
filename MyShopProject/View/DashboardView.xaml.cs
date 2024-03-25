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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShopProject.View
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : Page
    {
        public DashboardView()
        {
            InitializeComponent();
            DashboardViewModel current = new DashboardViewModel();
            quantityProductAvailable.Text = current._quantityProductAvailable.ToString();
            top5Product.ItemsSource = current._top5ProductSoldList;
            Global.GetActiveButton();
        }
    }
}
