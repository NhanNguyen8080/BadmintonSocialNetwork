using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BadmintonSocialNetwork.Repository.Validations;

namespace BadmintonSocialNetwork.Service.DTOs
{
    public class AccountDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
        [MaxLength(32, ErrorMessage = "Username cannot exceed 32 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [RegularExpression(@"^[\p{L}\s\-]+$", ErrorMessage = "Full Name can only contain letters, spaces, and hyphens.")]
        [MaxLength(32, ErrorMessage = "Full Name cannot exceed 32 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Gender must be 'Male', 'Female', or 'Other'.")]
        public string Gender { get; set; }
    }

    public class AccountVM : AccountDTO
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public int EmailOtp{ get; set; }
        public int PhoneNumberOtp { get; set; }
    }

    public class AccountCM : AccountDTO
    {

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [MaxLength(32, ErrorMessage = "Password cannot exceed 32 characters.")]
        [CustomPasswordValidation]
        public string Password { get; set; }
    }

    public class AccountUM : AccountDTO
    {

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [MaxLength(32, ErrorMessage = "Password cannot exceed 32 characters.")]
        [CustomPasswordValidation]
        public string Password { get; set; }
    }
}
