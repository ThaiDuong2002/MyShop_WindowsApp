using MyShopProject.Model;
using MyShopProject.Repositories;
using MyShopProject.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MyShopProject.ViewModel
{
    public class ProductCategoryManagementViewModel :BaseViewModel
    {
        public ProductCatgoryRepository _productCatgoryRepository;
        private ObservableCollection<Brand> _brands;
        public ObservableCollection<Brand> Brands
        {
            get => _brands;
            set
            {
                _brands = value;
                OnPropertyChanged();
            }
        }
        private Brand _selectedBrand;
        public Brand SelectedBrand
        {
            get => _selectedBrand;
            set
            {
                _selectedBrand = value;
                OnPropertyChanged();
            }
        }
        
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ProductCategoryManagementViewModel()
        {
            _productCatgoryRepository = new ProductCatgoryRepository();
            Brands = new ObservableCollection<Brand>(_productCatgoryRepository.GetAllBrands());
            AddCommand = new RelayCommand<object>((p) => { return true; }, (p) => {AddBrandWindow(); });
            EditCommand = new RelayCommand<object>((p) => { return CheckExcute(); }, (p) => { EditBrandWindow(); });
            DeleteCommand = new RelayCommand<object>((p) => { return CheckExcute(); }, (p) => { DeleteBrand(); });
        }

        public bool CheckExcute()
        {
            if (SelectedBrand == null)
            {
                return false;
            }
            return true;
        }

        public void AddBrandWindow()
        {
            AddBrandView addBrandView = new AddBrandView();
            if(addBrandView.ShowDialog()==true)
            {
                Brands.Clear();
                foreach (var item in _productCatgoryRepository.GetAllBrands())
                {
                    Brands.Add(item);
                }
            }    
        }

        public void EditBrandWindow()
        {
            EditBrandView editBrandView = new EditBrandView(SelectedBrand);
            if(editBrandView.ShowDialog() ==true)
            {
                var id= SelectedBrand.Id;
                Brands[id]= _productCatgoryRepository.GetBrandById(id);
            }    
        }
        
        public void DeleteBrand()
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa thương hiệu này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (_productCatgoryRepository.DeleteBrand(SelectedBrand.Id))
                {
                    Brands.Remove(SelectedBrand);
                    MessageBox.Show("Xóa thương hiệu thành công");
                }
                else
                {
                    MessageBox.Show("Xóa thương hiệu thất bại");
                }
            }
        }
    }
}
