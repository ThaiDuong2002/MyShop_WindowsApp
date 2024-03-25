using Microsoft.Win32;
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

namespace MyShopProject.UserControls
{
    /// <summary>
    /// Interaction logic for BrandUserControl.xaml
    /// </summary>
    public partial class BrandUserControl : UserControl
    {
        public BrandUserControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                fileName.Text = new Uri(openFileDialog.FileName).ToString();
            }
        }
    }
}
