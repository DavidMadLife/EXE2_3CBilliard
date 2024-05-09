using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Response;

public class NotificateResponse
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Descrpition { get; set; }
    public DateTime CreateAt { get; set; }
    public string Status { get; set; }
}
