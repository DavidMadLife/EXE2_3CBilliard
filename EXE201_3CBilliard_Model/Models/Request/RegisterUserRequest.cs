﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class RegisterUserRequest
    {
        [Required]
        public long RoleId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string IdentificationCardNumber { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        [Required]
        public DateTime ModifineAt { get; set; }
        [Required]
        public DateTime DoB { get; set; }
        [Required]
        public string Note { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
