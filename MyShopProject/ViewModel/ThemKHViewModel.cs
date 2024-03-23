using Microsoft.Win32;
using MyShopProject.Model;
using MyShopProject.repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MyShopProject.ViewModel
{
    public class ThemKHViewModel : BaseViewModel
    {

        private string _name;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }
        private string _phone;
        public string Phone { get => _phone; set { _phone = value; OnPropertyChanged(); } }
        private string _address;
        public string Address { get => _address; set { _address = value; OnPropertyChanged(); } }
        private DateTime _birthday = DateTime.Now;
        public DateTime Birthday { get => _birthday; set { _birthday = value; OnPropertyChanged(); } }

        private BitmapImage _image { get; set; }
        public BitmapImage Image { get => _image; set { _image = value; OnPropertyChanged(); } }

        


        
        public UserRepository userRepository = new UserRepository();
        public ICommand AddCustomerCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }

        public ThemKHViewModel()
        {
            AddCustomerCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                addCustomer();
            });

            ChooseImageCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChooseImage();
            });

        }
        public void addCustomer()
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
                Birthday = DateOnly.FromDateTime(Birthday)
            };
            if (userRepository.AddUser(user))
            {
                MessageBox.Show("Thêm khách hàng thành công");
                Application.Current.Windows[1].DialogResult = true;
            }
            else
            {
                MessageBox.Show("Thêm khách hàng thất bại");
            }

        }
        public void ChooseImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Image = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }
}
