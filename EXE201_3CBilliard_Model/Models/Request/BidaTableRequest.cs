using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Request;

public class BidaTableRequest
{
    [Required]
    public long BidaCludId { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public string TableName { get; set; }
    [Required]
    public string Image { get; set; }
    [Required]
    public DateTime CreateAt { get; set; }
    [Required]
    public string Note { get; set; }
    [Required]
    public string Status { get; set; }
}
