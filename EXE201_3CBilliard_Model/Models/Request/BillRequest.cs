using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class BillRequest
    {
        [Required(ErrorMessage = "OrderCode is required")]
        public string OrderCode { get; set; }
        [Required(ErrorMessage = "PaymentMethods is required")]
        public int PaymentMethods { get; set; }
        public string BookerName { get; set; }
        public string BookerPhone { get; set; }
        public string BookerEmail { get; set; }
    }
}
