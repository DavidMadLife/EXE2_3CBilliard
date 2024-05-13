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
        [Required]
        public NotificateStatus Status { get; set; }
    }
}
