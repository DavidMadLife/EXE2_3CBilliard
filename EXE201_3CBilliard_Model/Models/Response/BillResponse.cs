using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Response
{
    public class BillResponse
    {
        public long Id { get; set; }
        public long ClubId { get; set; }
        public string BookerName { get; set; }
        public string BookerPhone { get; set; }
        public string BookerEmail { get; set; }
        public int PaymentMethods { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime BookingDate { get; set; }
        public string OrderCode { get; set; }
        public string Descrpition { get; set; }
        public string Status { get; set; }
        public List<long> BookedSlotIds { get; set; } // Thêm danh sách ID của các slot đã được đặt
    }


}
