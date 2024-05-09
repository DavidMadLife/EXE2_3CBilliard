using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Request;


public class RoleRequest
{
    [Required]
    public string RoleName { get; set; }
    [Required]
    public string Status { get; set; }
}
