using LiveCharts;
using LiveCharts.Wpf;
using MyShopProject.Repositories;
using System.Windows.Input;

namespace MyShopProject.ViewModel
{
    public class RevenueStatisticsViewModel : BaseViewModel
    {
        private OrderRepository _orderRepository = new OrderRepository();
        private string[] _years = new string[] { "2020", "2021", "2022", "2023", "2024" };
        public string[] Years
        {
            get => _years;
            set
            {
                _years = value;
                OnPropertyChanged();
            }
        }

        private string[] _months = new string[]
        {
            "- Không chọn -",
            "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6",
            "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"
        };
        public string[] Months
        {
            get => _months;
            set
            {
                _months = value;
                OnPropertyChanged();
            }
        }

        private string[] _weeks = new string[]
        {
            "- Không chọn -", "Tuần 1", "Tuần 2", "Tuần 3", "Tuần 4", "Tuần 5"
        };
        public string[] Weeks
        {
            get => _weeks;
            set
            {
                _weeks = value;
                OnPropertyChanged();
            }
        }
        private string _selectedYear;
        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged();
            }
        }
        private string _selectedMonth;
        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                OnPropertyChanged();
            }
        }
        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged();
            }
        }
        private string _selectedWeek;
        public string SelectedWeek
        {
            get => _selectedWeek;
            set
            {
                _selectedWeek = value;
                OnPropertyChanged();
            }
        }
        private string[] _labels;
        public string[] Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                OnPropertyChanged();
            }
        }
        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }
        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }
        public Func<double, string> Formatter { get; set; } = value => value.ToString("C");
        public ICommand FilterByDateToDateCommand { get; set; }
        public ICommand FilterByYearMonthWeekCommand { get; set; }
        public RevenueStatisticsViewModel()
        {
            _selectedYear = Years[4];
            _selectedMonth = Months[0];
            _selectedWeek = Weeks[0];
            _startDate = DateTime.Now;
            _endDate = DateTime.Now;
            _labels = new string[] { };
            _seriesCollection = new SeriesCollection();
            FilterByDateToDateCommand = new RelayCommand<object>((p) => true, (p) => { FilterByDateToDate(); });
            FilterByYearMonthWeekCommand = new RelayCommand<object>((p) => true, (p) => { FilterByYearMonthWeek(); });
            LoadData(null, null, SelectedYear);
        }
        private void LoadData(DateTime? startDate = null, DateTime? endDate = null, string? Year = null, string? Month = null, string? Week = null)
        {
            (List<int> revenues, List<int> profits, List<string> dates) result;
            if (Year != null && Month != null && Week != null)
            {
                int year = int.Parse(Year);
                int month = Array.IndexOf(Months, Month);
                int week = Array.IndexOf(Weeks, Week);
                result = _orderRepository.GetRevenueAndProfitByWeek(year, month, week);
                SeriesCollection.Clear();
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "Doanh thu",
                    Values = new ChartValues<int>(result.revenues)
                });
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "Lợi nhuận",
                    Values = new ChartValues<int>(result.profits)
                });
                Labels = result.dates.ToArray();
                return;
            }
            if (Year != null && Month != null)
            {
                int year = int.Parse(Year);
                int month = Array.IndexOf(Months, Month);
                result = _orderRepository.GetRevenueAndProfitByMonth(year, month);
                SeriesCollection.Clear();
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "Doanh thu",
                    Values = new ChartValues<int>(result.revenues)
                });
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "Lợi nhuận",
                    Values = new ChartValues<int>(result.profits)
                });
                Labels = result.dates.ToArray();
                return;
            }
            if (Year != null)
            {
                int year = int.Parse(Year);
                result = _orderRepository.GetRevenueAndProfitByYear(year);
                SeriesCollection.Clear();
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "Doanh thu",
                    Values = new ChartValues<int>(result.revenues)
                });
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "Lợi nhuận",
                    Values = new ChartValues<int>(result.profits)
                });
                Labels = result.dates.ToArray();
                return;
            }
            if (startDate.HasValue && endDate.HasValue)
            {
                result = _orderRepository.GetRevenueAndProfitByDateToDate(StartDate, EndDate);
                SeriesCollection.Clear();
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "Doanh thu",
                    Values = new ChartValues<int>(result.revenues)
                });
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = "Lợi nhuận",
                    Values = new ChartValues<int>(result.profits)
                });
                Labels = result.dates.ToArray();
                return;
            }
            return;
        }
        private void FilterByDateToDate()
        {
            LoadData(StartDate, EndDate);
        }
        private void FilterByYearMonthWeek()
        {
            if (SelectedYear != "- Không chọn -" && SelectedMonth != "- Không chọn -" && SelectedWeek != "- Không chọn -")
            {
                LoadData(null, null, SelectedYear, SelectedMonth, SelectedWeek);
            }
            if (SelectedYear != "- Không chọn -" && SelectedMonth != "- Không chọn -" && SelectedWeek == "- Không chọn -")
            {
                LoadData(null, null, SelectedYear, SelectedMonth);
            }
            if (SelectedYear != "- Không chọn -" && SelectedMonth == "- Không chọn -" && SelectedWeek == "- Không chọn -")
            {
                LoadData(null, null, SelectedYear);
            }
        }
    }
}
