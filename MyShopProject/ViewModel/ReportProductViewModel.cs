using LiveCharts;
using LiveCharts.Wpf;
using MyShopProject.Repositories;
using System.Windows.Input;

namespace MyShopProject.ViewModel
{
    public class ReportProductViewModel : BaseViewModel
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
        private string _selectedLabel;
        public string SelectedLabel
        {
            get => _selectedLabel;
            set
            {
                _selectedLabel = value;
                OnPropertyChanged();
            }
        }

        public Func<double, string> Formatter { get; set; } = value => value.ToString("N");
        public ICommand FilterByYearMonthWeekCommand { get; set; }
        public ReportProductViewModel()
        {
            _selectedYear = Years[4];
            _selectedMonth = Months[0];
            _selectedWeek = Weeks[0];
            _labels = new string[] { };
            _selectedLabel = "";
            _seriesCollection = new SeriesCollection();
            FilterByYearMonthWeekCommand = new RelayCommand<object>((p) => true, (p) => { FilterByYearMonthWeek(); });
            LoadData(SelectedYear);
        }
        private void LoadData(string? Year = null, string? Month = null, string? Week = null)
        {
            Dictionary<string, int> result;
            (Dictionary<string, int> result, string labels) special;
            if (Year != null && Month != null && Week != null)
            {
                int year = int.Parse(Year);
                int month = Array.IndexOf(Months, Month);
                int week = Array.IndexOf(Weeks, Week);
                special = _orderRepository.GetTop5BestSalersProductByWeek(year, month, week);
                result = special.result;
                SeriesCollection.Clear();
                foreach (var item in result)
                {
                    SeriesCollection.Add(new PieSeries
                    {
                        Title = item.Key,
                        Values = new ChartValues<int> { item.Value },
                        DataLabels = true
                    });
                }
                SelectedLabel = $"{special.labels}";
                return;
            }
            if (Year != null && Month != null)
            {
                int year = int.Parse(Year);
                int month = Array.IndexOf(Months, Month);
                result = _orderRepository.GetTop5BestSalersProductByMonth(year, month);
                SeriesCollection.Clear();
                foreach (var item in result)
                {
                    SeriesCollection.Add(new PieSeries
                    {
                        Title = item.Key,
                        Values = new ChartValues<int> { item.Value },
                        DataLabels = true
                    });
                }
                SelectedLabel = $"Tháng {month}, {year}";
                return;
            }
            if (Year != null)
            {
                int year = int.Parse(Year);
                result = _orderRepository.GetTop5BestSalersProductByYear(year);
                SeriesCollection.Clear();
                foreach (var item in result)
                {
                    SeriesCollection.Add(new PieSeries
                    {
                        Title = item.Key,
                        Values = new ChartValues<int> { item.Value },
                        DataLabels = true
                    });
                }
                SelectedLabel = $"Năm {year}";
                return;
            }
            return;
        }
        private void FilterByYearMonthWeek()
        {
            if (SelectedYear != "- Không chọn -" && SelectedMonth != "- Không chọn -" && SelectedWeek != "- Không chọn -")
            {
                LoadData(SelectedYear, SelectedMonth, SelectedWeek);
            }
            if (SelectedYear != "- Không chọn -" && SelectedMonth != "- Không chọn -" && SelectedWeek == "- Không chọn -")
            {
                LoadData(SelectedYear, SelectedMonth);
            }
            if (SelectedYear != "- Không chọn -" && SelectedMonth == "- Không chọn -" && SelectedWeek == "- Không chọn -")
            {
                LoadData(SelectedYear);
            }
        }
    }
}
