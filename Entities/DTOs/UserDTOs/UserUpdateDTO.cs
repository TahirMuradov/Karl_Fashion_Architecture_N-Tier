using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs.UserDTOs
{
    public class UserUpdateDTO
    {
        public string userId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile no. is required")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        [Phone]

        public string PhoneNumber { get; set; }

        public string? newPassword { get; set; }
        [Compare("newPassword")]
        public string? ConfirmPassword { get; set; }

    }
}
