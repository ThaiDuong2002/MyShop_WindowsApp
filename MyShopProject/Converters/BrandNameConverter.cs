using System.Windows.Data;

namespace MyShopProject.Converters
{
    public class BrandNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value == "")
            {
                return "CHƯA PHÂN LOẠI";
            }
            return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
