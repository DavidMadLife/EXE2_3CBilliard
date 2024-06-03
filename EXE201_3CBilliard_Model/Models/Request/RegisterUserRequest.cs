using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class RegisterUserRequest
    {
        

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        /*[RegularExpression("^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$", ErrorMessage = "Invalid phone number format")]*/
        public string Phone { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public DateTime DoB { get; set; }
       
       
    }
}
