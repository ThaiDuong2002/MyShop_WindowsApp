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
        public ProductCategoryManagementViewModel()
        {
            _productCatgoryRepository = new ProductCatgoryRepository();
            Brands = new ObservableCollection<Brand>(_productCatgoryRepository.GetAllBrands());
            AddCommand = new RelayCommand<object>((p) => { return true; }, (p) => {AddBrandWindow(); });
            EditCommand = new RelayCommand<object>((p) => {
                if (SelectedBrand == null)
                {
                    return false;
                }
                return true;
            }, (p) => { EditBrandWindow(); });



        }

        public void AddBrandWindow()
        {
            AddBrandView addBrandView = new AddBrandView();
            addBrandView.ShowDialog();
        }

        public void EditBrandWindow()
        {
            EditBrandView editBrandView = new EditBrandView(SelectedBrand);
            editBrandView.ShowDialog();
        }   
    }
}
