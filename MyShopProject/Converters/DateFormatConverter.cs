using System.Windows.Data;

namespace MyShopProject.Converters
{
    public class DateFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "";

            if (value is DateTime)
            {
                DateTime date = (DateTime)value;
                return date.ToString("dd/MM/yyyy");
            }
            else
            {
                DateOnly date = (DateOnly)value;
                return date.ToString("dd/MM/yyyy");
            }



        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
