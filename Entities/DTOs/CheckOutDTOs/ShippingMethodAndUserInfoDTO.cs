using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.CheckOutDTOs
{
    public class ShippingMethodAndUserInfoDTO
    {
        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the phone number. Must not be empty and should adhere to the specified format.
        /// </summary>
        [Required(ErrorMessage = "Phone Number cannot be empty")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the email address. Should not be empty and must be a valid email format.
        /// </summary>
        [EmailAddress(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the address. Must not be empty.
        /// </summary>
        [Required(ErrorMessage = "Address cannot be empty")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the city. Must not be empty.
        /// </summary>
        [Required(ErrorMessage = "City cannot be empty")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// Gets or sets the shipping method ID.
        /// </summary>
        public string ShippingMethodId { get; set; }

        /// <summary>
        /// Gets or sets the message. Must not exceed 500 characters.
        /// </summary>
        [MaxLength(500, ErrorMessage = "Message should not exceed 500 characters")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has confirmed the data. Must be true.
        /// </summary>
        [Required(ErrorMessage = "You must agree to the Terms and Conditions")]

        public bool ConfirmedDataInUser { get; set; }

        public string PaymentsMethodId { get; set; }
    }
}
