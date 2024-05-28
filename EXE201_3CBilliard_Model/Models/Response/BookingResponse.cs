﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Response;

public class BookingResponse
{
    public long Id { get; set; }
    public long BT_SlotId { get; set; }
    public long UserId { get; set; }
    public double Price { get; set; }
    public DateTime CreateAt { get; set; }
    public string OrderCode { get; set; }
    public string Descrpition { get; set; }
    public string Note { get; set; }
    public string Status { get; set; }
}
