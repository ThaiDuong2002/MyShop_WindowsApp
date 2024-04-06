using MyShopProject.Utilities;
using System.Windows;
using System.Windows.Input;

namespace MyShopProject.ViewModel
{
    public class ConnectConfigViewModel : BaseViewModel
    {
        private string _ServerName;
        private string _DatabaseName;
        public string ServerName
        {
            get => _ServerName;
            set { _ServerName = value; OnPropertyChanged(); }
        }

        public string DatabaseName
        {
            get => _DatabaseName;
            set { _DatabaseName = value; OnPropertyChanged(); }
        }
        public ICommand AcceptConfigCommand { get; set; }
        public ICommand CloseConfigCommand { get; set; }

        public ConnectConfigViewModel()
        {
            _DatabaseName = "";
            _ServerName = "";
            var isRemember = SecurityConfig.getValueFromConfig("IsRemember") == "True";
            if (isRemember)
            {
                var serverName = SecurityConfig.getValueFromConfig("ServerName");
                var databaseName = SecurityConfig.getValueFromConfig("DatabaseName");
                ServerName = serverName;
                DatabaseName = databaseName;
            }
            AcceptConfigCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Accept(p); });
            CloseConfigCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
        }

        private void Accept(Window p)
        {
            if (p == null)
                return;
            if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(DatabaseName))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            }
            else
            {
                p.DialogResult = true;
                p.Close();
            }
        }
    }
}
