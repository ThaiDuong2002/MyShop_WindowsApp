using MyShopProject.Model;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace MyShopProject.View
{
    /// <summary>
    /// Interaction logic for DetailProductView.xaml
    /// </summary>
    public partial class DetailProductView : Page
    {
        public Page prePage;
        public DetailProductView(Product product, Page prePage)
        {
            InitializeComponent();
            DataContext = product;
            this.prePage = prePage;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(prePage);
        }
    }
}
