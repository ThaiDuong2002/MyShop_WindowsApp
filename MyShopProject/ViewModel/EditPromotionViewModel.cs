using MyShopProject.Model;
using MyShopProject.Repositories;
using System.Windows;
using System.Windows.Input;

namespace MyShopProject.ViewModel
{
    public class EditPromotionViewModel : BaseViewModel
    {
        public PromotionRepository _promotionRepository;

        private string _title = "Chỉnh sửa khuyến mãi";
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

        private int? _byPercent;
        public int? ByPercent
        {
            get { return _byPercent; }
            set
            {
                _byPercent = value;
                OnPropertyChanged(nameof(ByPercent));
            }
        }

        private int? _byCash;
        public int? ByCash
        {
            get { return _byCash; }
            set
            {
                _byCash = value;
                OnPropertyChanged(nameof(ByCash));
            }
        }

        private string? _byProduct;

        public string? ByProduct
        {
            get { return _byProduct; }
            set
            {
                _byProduct = value;
                OnPropertyChanged(nameof(ByProduct));
            }
        }

        private List<string> _categories = new List<string> { "Giảm theo phần trăm", "Giảm trực tiếp", "Tặng sản phẩm" };
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

        public ICommand SaveCommand { get; set; }
        public EditPromotionViewModel()
        {
            _promotionRepository = new PromotionRepository();
            SaveCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { UpdatePromotion(p); });

        }
        public int id;

        public void LoadPromotion(Promotion promotion)
        {
            id = promotion.Id;
            Name = promotion.Name;
            if (promotion.ByPercent.HasValue)
            {
                SelectedCategory = Categories[0];
                ByPercent = promotion.ByPercent.Value;
            }
            else if (promotion.ByCash.HasValue)
            {
                SelectedCategory = Categories[1];
                ByCash = promotion.ByCash.Value;

            }
            else
            {
                SelectedCategory = Categories[2];
                ByProduct = promotion.ByProduct;
            }

        }

        private void UpdatePromotion(Window p)
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
            if(SelectedCategory == Categories[0])
            {
                ByCash = null;
                ByProduct = null;
            }
            else if(SelectedCategory == Categories[1])
            {
                ByPercent = null;
                ByProduct = null;
            }
            else
            {
                ByPercent = null;
                ByCash = null;
            }
            var promotion = new Promotion()
            {
                Name = Name,
                ByPercent = ByPercent,
                ByCash = ByCash,
                ByProduct = ByProduct
            };
            if (_promotionRepository.UpdatePromotion(promotion, id))
            {
                System.Windows.MessageBox.Show("Cập nhật khuyến mãi thành công");
                p.DialogResult = true;
            }
            else
            {
                System.Windows.MessageBox.Show("Cập nhật khuyến mãi thất bại");
                p.DialogResult = false;

            }
        }
    }
}
