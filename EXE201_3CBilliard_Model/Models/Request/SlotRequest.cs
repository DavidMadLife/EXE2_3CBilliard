using System;
using System.ComponentModel.DataAnnotations;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class SlotRequest
    {
        [Required(ErrorMessage = "SlotTime is required")]
        public string SlotTime { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }
    }
}
