using System;
using System.ComponentModel.DataAnnotations;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class BookingRequest
    {
        [Required(ErrorMessage = "BT_SlotId is required")]
        public long BT_SlotId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "CreateAt is required")]
        public DateTime CreateAt { get; set; }
        [Required(ErrorMessage = "BookingDate is required")]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "OrderCode is required")]
        [RegularExpression(@"^[A-Z]{3}\d{7}$", ErrorMessage = "OrderCode must have 3 characters followed by 7 digits")]
        public string OrderCode { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        
    }
}
