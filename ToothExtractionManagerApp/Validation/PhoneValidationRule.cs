using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ToothExtractionManagerApp.Validation
{
    public class PhoneValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null || value.ToString().Length < 9)
            {
                return new ValidationResult(false, "Podaj minimum 9 cyfr.");
            }

            return long.TryParse(value.ToString(), out var i)
                ? new ValidationResult(true, null)
                : new ValidationResult(false, "Podaj jedynie cyfry.");
        }
    }
}
