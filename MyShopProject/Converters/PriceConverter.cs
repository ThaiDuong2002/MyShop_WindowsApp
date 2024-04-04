using System.Globalization;
using System.Windows.Data;

namespace MyShopProject.Converters
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                int price = (int)Math.Round(doubleValue);
                return price.ToString("N0", culture) + " VND";
            }
            else if (value is int intValue)
            {
                return intValue.ToString("N0", culture) + " VND";
            }
            else
            {
                return "0 VND";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;
            if (input != null)
            {
                // Xóa bỏ ký hiệu "VNĐ" trước khi chuyển đổi
                input = input.Replace(" VND", "");

                double result;
                if (double.TryParse(input, NumberStyles.AllowThousands, culture, out result))
                {
                    return result;
                }
            }
            return value;
        }
    }
}
