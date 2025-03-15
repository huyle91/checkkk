using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LeQuangHuyWPF.Utils
{
    public class RequiredFieldValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Field is required.")
                : ValidationResult.ValidResult;
        }
    }

    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = (value ?? "").ToString();
            if (string.IsNullOrWhiteSpace(email))
            {
                return new ValidationResult(false, "Email is required.");
            }

            // Simple regex for email validation
            var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(email)
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Please enter a valid email address.");
        }
    }

    public class PhoneValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string phone = (value ?? "").ToString();
            if (string.IsNullOrWhiteSpace(phone))
            {
                return new ValidationResult(false, "Phone number is required.");
            }

            // Simple regex for phone validation (digits, spaces, dashes, parentheses)
            var regex = new Regex(@"^[\d\s\-\(\)]{7,15}$");
            return regex.IsMatch(phone)
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Please enter a valid phone number.");
        }
    }

    public class NumericValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = (value ?? "").ToString();
            if (string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult(false, "Field is required.");
            }

            if (!decimal.TryParse(input, out _))
            {
                return new ValidationResult(false, "Please enter a valid number.");
            }

            return ValidationResult.ValidResult;
        }
    }

    public class PositiveNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = (value ?? "").ToString();
            if (string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult(false, "Field is required.");
            }

            if (!decimal.TryParse(input, out decimal number))
            {
                return new ValidationResult(false, "Please enter a valid number.");
            }

            return number > 0
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Please enter a positive number.");
        }
    }

    public class DateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = (value ?? "").ToString();
            if (string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult(false, "Date is required.");
            }

            if (!DateTime.TryParse(input, out _))
            {
                return new ValidationResult(false, "Please enter a valid date.");
            }

            return ValidationResult.ValidResult;
        }
    }

    public class PasswordValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string password = (value ?? "").ToString();
            if (string.IsNullOrWhiteSpace(password))
            {
                return new ValidationResult(false, "Password is required.");
            }

            if (password.Length < 6)
            {
                return new ValidationResult(false, "Password must be at least 6 characters long.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
