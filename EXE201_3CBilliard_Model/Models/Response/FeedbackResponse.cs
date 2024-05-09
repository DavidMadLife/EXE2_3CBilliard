using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Response;

public class FeedbackResponse
{
    public long FeedbackId { get; set; }
    public long UserId { get; set; }
    public long BidaClubId { get; set; }
    public string Descrpition { get; set; }
    public DateTime CreateAt { get; set; }
    public string Image { get; set; }
    public string Status { get; set; }
}
