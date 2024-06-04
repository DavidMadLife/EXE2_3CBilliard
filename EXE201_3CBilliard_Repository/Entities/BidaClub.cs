using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Entities
{
    public enum BidaClubStatus
    {
        ACTIVE,
        INACTIVE,
        WAITING,
        DELETED
    }

    [Table("BidaClub")]
    public class BidaClub
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string BidaName { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Email { get; set; }

        public string Descrpition { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        public string Note { get; set; }
        public BidaClubStatus Status { get; set; }
        /*[ForeignKey("UserId")]
        public User User { get; set; }*/


        // Giờ mở cửa
        [Required]
        public string OpeningHours { get; set; }

        // Giá tiền trung bình
        [Required]
        public decimal AveragePrice { get; set; }
    }
}
