using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class CombinedBookAndBillRequest
    {
        [Required(ErrorMessage = "UserId is required")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "BT_SlotIds is required")]
        public List<long> BT_SlotIds { get; set; }

        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "BookingDate is required")]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "OrderCode is required")]
        public string OrderCode { get; set; }

        [Required(ErrorMessage = "PaymentMethods is required")]
        public int PaymentMethods { get; set; }

        public string? BookerName { get; set; }
        public string? BookerPhone { get; set; }
        public string? BookerEmail { get; set; }
    }
}