using MyShopProject.Model;
using MyShopProject.Repositories;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace MyShopProject.ViewModel
{
    public class PromotionManagementViewModel : BaseViewModel
    {
        private int _currentPage = 1;
        private int _totalPage = 1;
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
        private string _amountPromotions;
        public string AmountPromotions { get => _amountPromotions; set { _amountPromotions = value; OnPropertyChanged(); } }
        public List<int> ItemsPerPage => new List<int> { 5, 10, 15, 20 };
        private int _selectedItemsPerPage = 5;
        public int SelectedItemsPerPage
        {
            get => _selectedItemsPerPage;
            set
            {
                _selectedItemsPerPage = value;
                OnPropertyChanged();
                LoadData();
            }
        }
        private string _searchPromotionText;
        public string SearchPromotionText
        {
            get { return _searchPromotionText; }
            set
            {
                if (_searchPromotionText != value)
                {
                    _searchPromotionText = value;
                    OnPropertyChanged(nameof(SearchPromotionText));

                    if (string.IsNullOrEmpty(_searchPromotionText))
                    {
                        LoadData();
                    }
                }
            }
        }
        private readonly PromotionRepository _promotionRepository = new PromotionRepository();
        private ObservableCollection<Promotion> _promotions;
        public ObservableCollection<Promotion> Promotions
        {
            get => _promotions;
            set
            {
                _promotions = value;
                OnPropertyChanged(nameof(Promotions));
            }
        }
        public ICommand prevBtn { get; set; }
        public ICommand nextBtn { get; set; }
        public ICommand KeyDownCommand { get; set; }
        public PromotionManagementViewModel()
        {
            _pageInfo = $"Trang {_currentPage} / {_totalPage}";
            _amountPromotions = "0 khuyến mãi";
            _promotions = new ObservableCollection<Promotion>();
            _searchPromotionText = "";
            prevBtn = new RelayCommand<object>((p) => { return _currentPage > 1; }, (p) => { _currentPage--; LoadData(); });
            nextBtn = new RelayCommand<object>((p) => { return _currentPage < _totalPage; }, (p) => { _currentPage++; LoadData(); });
            KeyDownCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadData(); });
            LoadData();
        }
        private void LoadData()
        {
            (int totalCount, ObservableCollection<Promotion> promotions) = _promotionRepository.GetPromotions(_currentPage, SelectedItemsPerPage, SearchPromotionText);
            Promotions = promotions;
            int totalPromotions = totalCount;
            Debug.WriteLine(totalPromotions);
            AmountPromotions = $"{totalPromotions} khuyến mãi";
            _totalPage = totalPromotions / SelectedItemsPerPage + (totalPromotions % SelectedItemsPerPage == 0 ? 0 : 1);
            PageInfo = $"Trang {_currentPage} / {_totalPage}";
        }
    }
}
