using System.Globalization;
using System.Windows.Data;

namespace MyShopProject.Converters
{
    public class PromotionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter.ToString() == "Giảm theo phần trăm")
            {
                if (value is int intValue)
                {
                    return value.ToString() + "%";
                }
                else
                {
                    return "0%";
                }
            }
            else
            {
              
                if (value is int intValue)
                {
                    return intValue.ToString("N0", culture) + " VND";
                }
                else
                {
                    return "0 VND";
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string input = value as string;
            if (input != null)
            {
                if (parameter.ToString() == "Giảm theo phần trăm")
                {
                    // Xóa bỏ ký hiệu "%" trước khi chuyển đổi
                    input = input.Replace("%", "");

                    int result;
                    if (int.TryParse(input, NumberStyles.AllowThousands, culture, out result))
                    {
                        return result;
                    }
                }
                else
                {
                    // Xóa bỏ ký hiệu "VNĐ" trước khi chuyển đổi
                    input = input.Replace(" VND", "");

                    int result;
                    if (int.TryParse(input, NumberStyles.AllowThousands, culture, out result))
                    {
                        return result;
                    }
                }
            }
            return value;
        }
    }
}
