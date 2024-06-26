﻿using Microsoft.IdentityModel.Tokens;
using MyShopProject.Model;
using MyShopProject.Repositories;
using MyShopProject.View;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MyShopProject.ViewModel
{
    public class QLKHViewModel : BaseViewModel
    {
        private string _searchCustomerText;
        public string SearchCustomerText
        {
            get { return _searchCustomerText; }
            set
            {
                if (_searchCustomerText != value) // Kiểm tra xem giá trị đã thay đổi hay không
                {
                    _searchCustomerText = value;
                    OnPropertyChanged(nameof(SearchCustomerText));

                    if (string.IsNullOrEmpty(_searchCustomerText)) // Kiểm tra xem SearchText có bị xóa không
                    {
                        LoadData(); // Nếu đã xóa, tải lại dữ liệu ban đầu
                    }
                }
            }
        }


        private string _pageInfo;
        public string PageInfo
        {
            get { return _pageInfo; }
            set
            {
                _pageInfo = value;
                OnPropertyChanged(nameof(PageInfo));
            }
        }


        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        private readonly UserRepository _userRepository = new UserRepository();
        private int _currentPage = 1;
        private int _totalPage = 1;

        public ICommand prevBtn { get; set; }
        public ICommand nextBtn { get; set; }
        public ICommand searchBtn { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand AddCommand { get; set; }


        private string _amountProduct;
        public string AmountProduct { get => _amountProduct; set { _amountProduct = value; OnPropertyChanged(); } }

        public QLKHViewModel()
        {
            prevBtn = new RelayCommand<object>((p) => { return _currentPage > 1; }, (p) => { Prev(); });
            nextBtn = new RelayCommand<object>((p) => { return _currentPage < _totalPage; }, (p) => { Next(); });
            searchBtn = new RelayCommand<object>((p) => { return !string.IsNullOrEmpty(SearchCustomerText); }, (p) => { Search(); });
            AddCommand = new RelayCommand<object>((p) => { return true; }, (p) => { ShowWindow(); });
            DeleteCommand = new RelayCommand<User>((user) => { return user != null; }, (user) => { DeleteUser(user); });
            EditCommand = new RelayCommand<User>((user) => { return user != null; }, (user) => { EditUser(user); });
            LoadData();
            Global.SaveScreen("QLKH");
        }


        public void LoadData()
        {
            if (!Users.IsNullOrEmpty() || _totalPage == 0)
            {
                Users.Clear(); // Xóa danh sách người dùng cũ
                var product = _userRepository.GetUsersByPagination(_currentPage, 10);
                if (product.IsNullOrEmpty())
                {
                    _currentPage--;
                }
                foreach (var user in _userRepository.GetUsersByPagination(_currentPage, 10))
                {
                    Users.Add(user); // Thêm người dùng mới vào danh sách
                }
            }
            else
            {
                Users = new ObservableCollection<User>(_userRepository.GetUsersByPagination(_currentPage, 10));
                int numOfUsers = _userRepository.getNumOfUsers();// Lấy số lượng người dùng
                _totalPage = numOfUsers / 10 + (numOfUsers % 10 == 0 ? 0 : 1); // Tính toán số trang

            }
            var totalUsers = _userRepository.getNumOfUsers();
            AmountProduct = totalUsers + " khách hàng";
            _totalPage = totalUsers / 10 + (totalUsers % 10 == 0 ? 0 : 1); // Tính toán số trang
            PageInfo = $"Trang {_currentPage}/{_totalPage}";


        }

        public void Prev()
        {
            _currentPage--;
            LoadData();
        }

        public void Next()
        {
            _currentPage++;
            LoadData();
        }

        public void Search()
        {
            _currentPage = 1;
            Users.Clear();
            foreach (var user in _userRepository.GetUsersByName(SearchCustomerText))
            {
                Users.Add(user);
            }
            int numOfUsers = Users.Count;// Lấy số lượng người dùng
            _totalPage = numOfUsers / 10 + (numOfUsers % 10 == 0 ? 0 : 1); // Tính toán số trang
            PageInfo = $"Trang {_currentPage}/{_totalPage}";
        }

        public void ShowWindow()
        {
            ThemKHView themKHView = new ThemKHView();
            if (themKHView.ShowDialog() == true)
            {
                LoadData();
            }          
        }

        public void EditUser(User user)
        {
            ChinhSuaKHView chinhSuaKHView = new ChinhSuaKHView(user.Id);
            if (chinhSuaKHView.ShowDialog() == true)
            {
                var index = Users.IndexOf(user);
                Users[index] = _userRepository.GetUserByID(user.Id);
                AmountProduct = _userRepository.getNumOfUsers() + " khách hàng";
            }
        }

        public void DeleteUser(User user)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                if (_userRepository.deleteUser(user))
                {
                    MessageBox.Show("Xóa khách hàng thành công");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Xóa khách hàng thất bại");
                }
            }
        }

    }
}

