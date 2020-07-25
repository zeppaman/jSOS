using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace JsOS.APP.Core
{
    public class IpValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if ( Regex.IsMatch("", value.ToString()))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Invalid_Address");
            }
        }
    }
}
