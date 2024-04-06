using Microsoft.IdentityModel.Tokens;
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
    public class AddPromotionViewModel :BaseViewModel
    {
        public PromotionRepository _promotionRepository;

        private string _title = "Thêm khuyến mãi";
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private int _byPercent;
        public int ByPercent
        {
            get { return _byPercent; }
            set
            {
                _byPercent = value;
                OnPropertyChanged(nameof(ByPercent));
            }
        }

        private int _byCash;
        public int ByCash
        {
            get { return _byCash; }
            set
            {
                _byCash = value;
                OnPropertyChanged(nameof(ByCash));
            }
        }

        private string _byProduct;

        public string ByProduct
        {
            get { return _byProduct; }
            set
            {
                _byProduct = value;
                OnPropertyChanged(nameof(ByProduct));
            }
        }

        private List<string> _categories = new List<string> { "Giảm theo phần trăm", "Giảm trực tiếp","Tặng sản phẩm" };
        public List<string> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        private string _selectedcategory;
        public string SelectedCategory
        {
            get { return _selectedcategory; }
            set
            {
                _selectedcategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }


        public ICommand AddCommand { get; set; }

        public AddPromotionViewModel()
        {
            _promotionRepository = new PromotionRepository();
            AddCommand = new RelayCommand<Window>((p) => { return !SelectedCategory.IsNullOrEmpty(); }, (p) => { AddPromotion(p); });

        }

        private void AddPromotion(Window p)
        {
            if (string.IsNullOrEmpty(Name))
            {
                System.Windows.MessageBox.Show("Vui lòng nhập tên khuyến mãi");
                return;
            }
            if (ByPercent == 0 && ByCash == 0 && string.IsNullOrEmpty(ByProduct))
            {
                System.Windows.MessageBox.Show("Vui lòng nhập giá trị khuyến mãi");
                return;
            }
            var promotion = new Promotion()
            {
                Name = Name,
                ByPercent = ByPercent,
                ByCash = ByCash,
                ByProduct = ByProduct,
            };
            if (_promotionRepository.AddPromotion(promotion))
            {
                System.Windows.MessageBox.Show("Thêm khuyến mãi thành công");
                p.DialogResult = true;

            }
            else
            {
                System.Windows.MessageBox.Show("Thêm khuyến mãi thất bại");
            }
        }

    }
}
