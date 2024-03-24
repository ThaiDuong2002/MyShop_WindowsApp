using MyShopProject.Model;
using MyShopProject.Repositories;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Reflection;


namespace MyShopProject.ViewModel
{
    public class ChinhSuaKHViewModel : BaseViewModel
    {

        private string _name;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        private string _phone;
        public string Phone { get => _phone; set { _phone = value; OnPropertyChanged(); } }
        private string _address;
        public string Address { get => _address; set { _address = value; OnPropertyChanged(); } }
        private string _username;
        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        private DateTime _birthday = DateTime.Now;
        public DateTime Birthday { get => _birthday; set { _birthday = value; OnPropertyChanged(); } }

        private BitmapImage _image { get; set; }
        public BitmapImage Image { get => _image; set { _image = value; OnPropertyChanged(); } }

        private byte[] _imageByte;
        public byte[] ImageByte { get => _imageByte; set { _imageByte = value; OnPropertyChanged(); } }

        public OpenFileDialog openFileDialog;

        public int Id { get; set; }

        public ICommand SaveCustomerCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }

        public ChinhSuaKHViewModel()
        {
            SaveCustomerCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                SaveCustomer();
            });

            ChooseImageCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChooseImage();
            });
        }
        public void SaveCustomer()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Address))
            {
                System.Windows.MessageBox.Show("Vui lòng nhập đủ thông tin");
                return;
            }
            var user = new User()
            {
                Name = Name,
                Phone = Phone,
                Address = Address,
                Birthday = DateOnly.FromDateTime(Birthday),
                Avatar = ImageByte
            };
            if (new UserRepository().updateUser(user, Id))
            {

                System.Windows.MessageBox.Show("Cập nhật thông tin khách hàng thành công");
                Application.Current.Windows[1].DialogResult = true;
            }
            else
            {
                System.Windows.MessageBox.Show("Cập nhật thông tin khách hàng thất bại");
            }
        }
        public void Load(int id)
        {
            
            Id = id;
            var customer = new UserRepository().GetUserByID(id);
            Name = customer.Name;
            Phone = customer.Phone;
            Address = customer.Address;
            Birthday = customer.Birthday.ToDateTime(TimeOnly.MinValue);
            Username = customer.UserName;
            if (customer.Avatar != null)
            {
                ImageByte = customer.Avatar;

                Stream StreamObj = new MemoryStream(ImageByte);
                BitmapImage BitObj = new BitmapImage();
                BitObj.BeginInit();
                BitObj.StreamSource = StreamObj;
                BitObj.EndInit();
                Image = BitObj;
            }
        }
        public void ChooseImage()
        {
            openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Image = new BitmapImage(new Uri(openFileDialog.FileName));

                ImageByte = File.ReadAllBytes(openFileDialog.FileName);


            }
        }
    }
}
