using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Entities
{
    public enum BookingStatus
    {
        ACTIVE,
        INACTIVE,
        WAITING,
        DELETED
    }

    [Table("Booking")]
    public class Booking
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long BT_SlotId { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        [Required]
        public DateTime BookingDate { get; set; }
        [Required]
        public string OrderCode { get; set; }
        [Required]
        public string Descrpition { get; set; }
        public string Note { get; set; }
        public double Price { get; set; }
        public BookingStatus Status { get; set; }
        [ForeignKey("BT_SlotId")]
        public BidaTable_Slot BSlot { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
