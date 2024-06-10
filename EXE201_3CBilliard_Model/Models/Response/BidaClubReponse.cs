using System;

namespace EXE201_3CBilliard_Model.Models.Response
{
    public class BidaClubReponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string BidaName { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public DateTime CreateAt { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public TimeSpan OpenTime { get; set; } // Giờ mở cửa
        public TimeSpan CloseTime { get; set; } // Giờ đóng cửa
        public decimal AveragePrice { get; set; }
    }
}
