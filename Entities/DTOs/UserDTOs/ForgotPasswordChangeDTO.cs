using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.UserDTOs
{
    public class ForgotPasswordChangeDTO
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        [Compare("ConfirmForgotPasswordChange",ErrorMessage ="Sifreler Ustu-uste dusmur ")]
        [Required(ErrorMessage = "Password is Doldurun")]
        public string ForgotPasswordChange { get; set; }
        [Required(ErrorMessage = "Confirm Password Doldurun")]

        public string ConfirmForgotPasswordChange { get; set; }
    }
}
