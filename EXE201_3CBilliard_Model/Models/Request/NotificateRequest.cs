using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Request;

public class NotificateRequest
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Descrpition { get; set; }
    [Required]
    public DateTime CreateAt { get; set; }
    [Required]
    public string Status { get; set; }
}
