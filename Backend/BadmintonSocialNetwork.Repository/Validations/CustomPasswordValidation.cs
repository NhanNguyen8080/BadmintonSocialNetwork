using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BadmintonSocialNetwork.Repository.Validations
{
    public class CustomPasswordValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string password)
            {
                var hasUpperCase = new Regex(@"[A-Z]");
                var hasLowerCase = new Regex(@"[a-z]");
                var hasDigit = new Regex(@"\d");
                var hasSpecialChar = new Regex(@"[\W_]");

                if (!hasUpperCase.IsMatch(password))
                    return new ValidationResult("Password must contain at least one uppercase letter.");

                if (!hasLowerCase.IsMatch(password))
                    return new ValidationResult("Password must contain at least one lowercase letter.");

                if (!hasDigit.IsMatch(password))
                    return new ValidationResult("Password must contain at least one digit.");

                if (!hasSpecialChar.IsMatch(password))
                    return new ValidationResult("Password must contain at least one special character (!@#$%^&* etc.).");
            }

            return ValidationResult.Success;
        }
    }

}
