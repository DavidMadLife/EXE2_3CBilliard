using System;
using System.ComponentModel.DataAnnotations;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class BidaClubRequest
    {
        [Required(ErrorMessage = "UserId is required")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "BidaName is required")]
        public string BidaName { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [PhoneValidation(ErrorMessage = "Phone must be between 8 and 11 digits and contain only numbers.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "OpeningHours is required")]
        [RegularExpression(@"^[0-9]{1,2}:[0-5][0-9] - [0-9]{1,2}:[0-5][0-9]$", ErrorMessage = "OpeningHours must be in the format 'HH:mm - HH:mm'.")]
        public string OpeningHours { get; set; }

    }

    // Custom validation attribute for phone number
    public class PhoneValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phone = value as string;
            if (phone == null)
            {
                // Null phone numbers are handled by the Required attribute
                return ValidationResult.Success;
            }

            // Validate phone number format
            if (!IsValidPhoneFormat(phone))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        private bool IsValidPhoneFormat(string phone)
        {
            // Remove non-digit characters from phone number
            string cleanedPhone = new string(phone.Where(char.IsDigit).ToArray());

            // Check if the phone number has a valid length and contains only digits
            return cleanedPhone.Length >= 8 && cleanedPhone.Length <= 11;
        }
    }
}
