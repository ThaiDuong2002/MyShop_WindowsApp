﻿using System.Globalization;
using System.Windows.Controls;

namespace MyShopProject.Validation
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Vui lòng điền thông tin")
                : ValidationResult.ValidResult;
        }
    }
}
