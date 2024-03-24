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

        public ICommand CloseCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand ConfigCommand { get; set; }

        public LoginViewModel()
        {
            IsLogin = false;
            IsRemember = false;
            _UserName = "";
            _Password = "";
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            ConfigCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Config(p); });
        }

        void Login(Window p)
        {
            if (p == null)
                return;
            MessageBox.Show("Login successfully");
            MessageBox.Show(_UserName + _Password);
            IsLogin = true;
            p.Close();
        }

        void Config(Window p)
        {
            if (p == null)
                return;
            MessageBox.Show("Config");
        }
    }
}
