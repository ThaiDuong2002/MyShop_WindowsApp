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
                    throw new ArgumentException("Password cannot be empty");
                else if (value.Length < 6)
                    throw new ArgumentException("Password must be at least 6 characters long");
                _Password = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            IsLogin = false;
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
        }

        void Login(Window p)
        {
            if (p == null)
                return;
            IsLogin = true;
            p.Close();
        }
    }
}
