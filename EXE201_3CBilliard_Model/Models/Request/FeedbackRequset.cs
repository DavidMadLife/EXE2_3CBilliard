using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Request;

public class FeedbackRequset
{
    [Required]
    public long UserId { get; set; }
    [Required]
    public long BidaClubId { get; set; }
    [Required]
    public string Descrpition { get; set; }
    [Required]
    public DateTime CreateAt { get; set; }
    [Required]
    public string Image { get; set; }
    [Required]
    public string Status { get; set; }
}
