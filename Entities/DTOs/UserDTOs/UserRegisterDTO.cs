using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Entities.DTOs.UserDTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "First Name is required")]

        public string Firstname { get; set; }
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]

        public string Lastname { get; set; }
        [Required(ErrorMessage = "Email is required")]

        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile no. is required")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Compare("ConfirmPassword")]
        [Required(ErrorMessage = "Password is required")]

        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]

        public string ConfirmPassword { get; set; }
       
    }
}
