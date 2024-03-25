using MyShopProject.Model;
using MyShopProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyShopProject.ViewModel
{
    public class AddBrandViewModel : BaseViewModel
    {

        public ICommand AddCommand { get; set; }
        public ProductCatgoryRepository _productCatgoryRepository = new ProductCatgoryRepository();
        public AddBrandViewModel()
        {

            /* AddCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
             {
                AddBrand();
             });*/

        }

        public void AddBrand(Brand brand)
        {
            /*if (string.IsNullOrEmpty(Name))
            {
                System.Windows.MessageBox.Show("Vui lòng nhập tên thương hiệu");
                return;
            }
            if (Logo == null)
            {
                System.Windows.MessageBox.Show("Vui lòng chọn logo");
                return;
            }
            var brand = new Brand()
            {
                Name = Name,
                Logo = Logo
            };*/
            if (_productCatgoryRepository.AddBrand(brand))
            {
                System.Windows.MessageBox.Show("Thêm thương hiệu thành công");
                Application.Current.Windows[1].DialogResult = true;
            }
            else
            {
                System.Windows.MessageBox.Show("Thêm thương hiệu thất bại");
            }
            Name = "";
            Logo = null;
        }

        private string _name;
        public string Name { get => _name; set { _name = value; OnPropertyChanged(); } }

        private string _logo;
        public string Logo { get => _logo; set { _logo = value; OnPropertyChanged(); } }


    }
}
