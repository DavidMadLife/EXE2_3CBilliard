using System;
using System.ComponentModel.DataAnnotations;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "RoleId is required")]
        public long RoleId { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression("^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$", ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "IdentificationCardNumber is required")]
        [RegularExpression("^0(0[1-9]|[1-8][0-9]|9[0-6])[0-3]([0-9][0-9])[0-9]{6}$", ErrorMessage = "Invalid IdentificationCardNumber format")]
        public string IdentificationCardNumber { get; set; }

        [Required(ErrorMessage = "Image is required")]
        /*[Url(ErrorMessage = "Image must be a valid URL")]*/
        public string Image { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "CreateAt is required")]
        public DateTime CreateAt { get; set; }

        [Required(ErrorMessage = "ModifineAt is required")]
        public DateTime ModifyAt { get; set; }

        [Required(ErrorMessage = "DoB is required")]
        public DateTime DoB { get; set; }

        [Required(ErrorMessage = "Note is required")]
        public string Note { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }
    }
}
