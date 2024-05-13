using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Entities
{
    public enum FeedbackStatus
    {
        ACTIVE,
        INACTIVE,
        WAITING,
        DELETED
    }
    [Table("Feedback")]
    public class Feedback
    {
        [Key]
        public long FeedbackId { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public long BidaClubId { get; set; }
        [Required]
        public string Descrpition { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public FeedbackStatus Status { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("BidaClubId")]
        public BidaClub BidaClub { get; set; }
    }
}
