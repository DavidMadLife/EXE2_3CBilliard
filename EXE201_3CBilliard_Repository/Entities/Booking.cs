using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Entities
{
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
        public string OrderCode { get; set; }
        [Required]
        public string Descrpition { get; set; }
        [Required]
        public string Note { get; set; }
        [Required]
        public string Status { get; set; }
        [ForeignKey("BT_SlotId")]
        public BidaTable_Slot Slot { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
