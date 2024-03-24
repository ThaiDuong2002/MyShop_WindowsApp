using MyShopProject.Model;
using MyShopProject.Utilities;
using MyShopProject.View;
using System.Configuration;
using System.Windows;
using System.Windows.Input;

namespace MyShopProject.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public bool IsLogin { get; set; }
        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password
        {
            get => _Password;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Vui lòng điền mật khẩu");
                if (value.Length < 6)
                    throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự");
                _Password = value;
                OnPropertyChanged();
            }
        }
        public bool IsRemember { get; set; }
        private string ServerName;
        private string DatabaseName;

        public ICommand CloseCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand ConfigCommand { get; set; }

        public LoginViewModel()
        {
            IsLogin = false;

            IsRemember = false;
            _Password = "";
            _UserName = "";
            ServerName = "";
            DatabaseName = "";

            if (ConfigurationManager.AppSettings["appSettings"] != null)
            {
                var IsRemember = SecurityConfig.getValueFromConfig("IsRemember") == "True";
                var _UserName = SecurityConfig.getValueFromConfig("UserName");
                var configEntropy = Convert.FromBase64String(SecurityConfig.getValueFromConfig("Entropy"));
                var password = SecurityConfig.getValueFromConfig("Password");
                var _Password = SecurityConfig.DecryptString(password, configEntropy);
                var ServerName = SecurityConfig.getValueFromConfig("ServerName");
                var DatabaseName = SecurityConfig.getValueFromConfig("DatabaseName");
            }

            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            ConfigCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Config(p); });
        }

        void Login(Window p)
        {
            if (p == null)
                return;

            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(DatabaseName))
            {
                MessageBox.Show("Vui lòng cấu hình kết nối", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MyShopContext.UserName = UserName;
            MyShopContext.Password = Password;
            MyShopContext.ServerName = ServerName;
            MyShopContext.DatabaseName = DatabaseName;

            MyShopContext myShopContext = new MyShopContext();
            if (myShopContext.Database.CanConnect())
            {
                if (IsRemember)
                {
                    var entropy = SecurityConfig.GenerateEntropy();
                    string encryptedPassword = SecurityConfig.EncryptString(Password, entropy);
                    SecurityConfig.saveToConfig(UserName, encryptedPassword, ServerName, DatabaseName, Convert.ToBase64String(entropy), IsRemember);
                }
                else
                {
                    SecurityConfig.removeAllConfig();
                }

                Dashboard dashboard = new Dashboard();
                dashboard.Show();
                IsLogin = true;
                p.Close();
            }
            else
            {
                MessageBox.Show("Kết nối thất bại");
                return;
            }
        }

        void Config(Window p)
        {
            if (p == null)
                return;
            ConnectConfigView configView = new ConnectConfigView();
            var connectConfigVM = configView.DataContext as ConnectConfigViewModel;

            if (configView.ShowDialog() == true)
            {
                if (connectConfigVM != null)
                {
                    ServerName = connectConfigVM.ServerName;
                    DatabaseName = connectConfigVM.DatabaseName;
                }
                else
                {
                    ServerName = "";
                    DatabaseName = "";
                }
            }
        }
    }
}
