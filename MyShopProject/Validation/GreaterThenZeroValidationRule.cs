using System.Globalization;
using System.Windows.Controls;

namespace MyShopProject.Validation
{
    public class GreaterThenZeroValidationRule : NotEmptyValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var result = base.Validate(value, cultureInfo);
            if (!result.IsValid)
            {
                return result;
            }

            return int.TryParse((value ?? "").ToString(), out var number) && number > 0
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Số lượng phải lớn hơn 0");
        }
    }
}
