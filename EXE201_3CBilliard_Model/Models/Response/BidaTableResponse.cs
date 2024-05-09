using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Response;

public class BidaTableResponse
{
    public long Id { get; set; }
    public long BidaCludId { get; set; }
    public double Price { get; set; }
    public string TableName { get; set; }
    public string Image { get; set; }
    public DateTime CreateAt { get; set; }
    public string Note { get; set; }
    public string Status { get; set; }
}

