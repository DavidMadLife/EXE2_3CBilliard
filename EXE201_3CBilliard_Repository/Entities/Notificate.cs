using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Entities
{
    public enum NotificateStatus
    {
        ACTIVE,
        INACTIVE,
        WAITING,
        DELETED
    }

    public enum NotificationType
    {
        PostNotification = 1, // Thông báo về bài viết
        ClubNotification = 2, // Thông báo về CLB
        BookingNotification = 3 // Thông báo về booking
    }


    [Table("Notificate")]
    public class Notificate
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        [Required]
        public string Descrpition { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        public NotificateStatus Status { get; set; }

        [Required]
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public NotificationType Type { get; set; } // Thêm trường này
    }
}
