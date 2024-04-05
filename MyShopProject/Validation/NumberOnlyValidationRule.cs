using System.Windows.Controls;

namespace MyShopProject.Validation
{
    public class NumberOnlyValidationRule : NotEmptyValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var result = base.Validate(value, cultureInfo);
            if (!result.IsValid)
            {
                return result;
            }

            return int.TryParse((value ?? "").ToString(), out var number)
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Chỉ được nhập số");
        }
    }
}
