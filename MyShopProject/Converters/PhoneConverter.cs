using System.Globalization;
using System.Windows.Data;

namespace MyShopProject.Converters
{
    public class PhoneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? phone = value as string;
            if (phone == null)
            {
                return null;
            }

            if (phone.Length == 10)
            {
                return string.Format("({0}) {1}-{2}", phone.Substring(0, 4), phone.Substring(4, 3), phone.Substring(7, 3));
            }
            else if (phone.Length == 7)
            {
                return string.Format("{0}-{1}", phone.Substring(0, 3), phone.Substring(3, 4));
            }
            else
            {
                return phone;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? phone = value as string;
            if (phone == null)
            {
                return null;
            }

            return phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
        }
    }
}
