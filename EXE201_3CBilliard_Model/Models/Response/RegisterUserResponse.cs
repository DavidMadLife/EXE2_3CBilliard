using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Response
{
    public class RegisterUserResponse
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string IdentificationCardNumber { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime ModifineAt { get; set; }
        public DateTime DoB { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
    }
}
