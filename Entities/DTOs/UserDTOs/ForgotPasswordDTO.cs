using System.ComponentModel.DataAnnotations;

namespace Entities.DTOs.UserDTOs
{
    public class ForgotPasswordDTO
    {
        [EmailAddress(ErrorMessage ="Duzgun Email Daxil edin")]
        [Required(ErrorMessage ="Email Daxil Edin")]
        public string Email { get; set; }
    }
}
