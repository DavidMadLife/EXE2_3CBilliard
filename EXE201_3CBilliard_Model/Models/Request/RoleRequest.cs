using System;
using System.ComponentModel.DataAnnotations;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class RoleRequest
    {
        [Required(ErrorMessage = "RoleName is required")]
        public string RoleName { get; set; }

        
    }
}
