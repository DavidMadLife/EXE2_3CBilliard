using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Entities
{
    public enum BidaTable_SlotStatus
    {
        ACTIVE,
        INACTIVE,
        WAITING,
        DELETED
    }

    [Table("BidaTable_Slot")]
    public class BidaTable_Slot
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long SlotId { get; set; }
        [Required]
        public long BidaTableId { get; set; }
        public BidaTable_SlotStatus Status { get; set; }
        [ForeignKey("SlotId")]
        public Slot Slot { get; set; }
        [ForeignKey("BidaTableId")]
        public BidaTable BidaTable { get; set; }
    }
}
