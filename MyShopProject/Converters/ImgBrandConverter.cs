using System;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MyShopProject.Converters
{
    public class ImgBrandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "/Images/user/default.jpg";
            }
            return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
