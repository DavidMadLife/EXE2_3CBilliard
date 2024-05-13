using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Entities
{
    public enum BidaTableStatus
    {
        ACTIVE,
        INACTIVE,
        WAITING,
        DELETED
    }

    [Table("BidaTable")]
    public class BidaTable
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long BidaCludId { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string TableName { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        [Required]
        public string Note { get; set; }
        [Required]
        public BidaTableStatus Status { get; set; }
        [ForeignKey("BidaCludId")]
        public BidaClub BidaClub { get; set; }
    }
}
