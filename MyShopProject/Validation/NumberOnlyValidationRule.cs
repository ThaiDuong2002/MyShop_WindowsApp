using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace MyShopProject.Validation
{
    public class NumberOnlyValidationRule : NotEmptyValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            // Chuyển đổi giá trị thành chuỗi
            string inputString = (value ?? "").ToString();

            // Kiểm tra xem chuỗi có trống hoặc null không
            if (string.IsNullOrWhiteSpace(inputString))
            {
                return new ValidationResult(false, "Vui lòng nhập số điện thoại");
            }
            // Kiểm tra xem chuỗi nhập vào có phải là số hay không
            else if (!Regex.IsMatch(inputString, @"^\d+$")) // Nếu không phải số
            {
                return new ValidationResult(false, "Bắt buộc phải nhập số");
            }
            // Nếu qua cả hai kiểm tra trên, tức là giá trị hợp lệ
            return ValidationResult.ValidResult;


        }
    }
}
