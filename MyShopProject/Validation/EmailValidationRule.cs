using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace MyShopProject.Validation
{
    public class EmailValidationRule : NotEmptyValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var result = base.Validate(value, cultureInfo);
            if (!result.IsValid)
            {
                return result;
            }

            return Regex.IsMatch((value ?? "").ToString(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Email không hợp lệ");
        }
    }
}
