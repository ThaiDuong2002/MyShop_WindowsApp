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
    /// Interaction logic for BackupRestoreDBView.xaml
    /// </summary>
    public partial class BackupRestoreDBView : Page
    {
        public BackupRestoreDBView()
        {
            InitializeComponent();
            Global.GetActiveButton();

            Global.SaveScreen("DB");
        }
    }
}
