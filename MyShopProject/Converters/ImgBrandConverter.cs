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
            if (value is byte[] byteArray && byteArray.Length > 0)
            {
                using (MemoryStream stream = new MemoryStream(byteArray))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                    return image;
                }
            }
            else if (value is string stringValue && !string.IsNullOrEmpty(stringValue))
            {
                // Assuming value is the filename
                string imagePath = $"/Images/brand/brand{stringValue}.jpg";
                return new BitmapImage(new Uri(imagePath, UriKind.Relative));
            }

            // Return a default image or handle accordingly
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
