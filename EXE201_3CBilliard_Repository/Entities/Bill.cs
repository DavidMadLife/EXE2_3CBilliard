using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Entities
{
    public enum BillStatus
    {
        ACTIVE,
        INACTIVE,
        WAITING,
        DELETED
    }

    [Table("Bill")]
    public class Bill
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public int PaymentMethods { get; set; }
        
        public string BookerName { get; set; }
        
        public string BookerPhone { get; set; }
        
        public string BookerEmail { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        public DateTime BookingDate { get; set; }
        [Required]
        public string OrderCode { get; set; }
        [Required]
        public string Descrpition { get; set; }
        public double Price { get; set; }
        public BillStatus Status { get; set; }
        
    }
}
