using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class BookAndBillRequest
    {
        [Required(ErrorMessage = "UserId is required")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "BT_SlotIds is required")]
        public List<long> BT_SlotIds { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "BookingDate is required")]
        public DateTime BookingDate { get; set; }

        [Required]
        public BillRequest BillRequest { get; set; }
    }
}
