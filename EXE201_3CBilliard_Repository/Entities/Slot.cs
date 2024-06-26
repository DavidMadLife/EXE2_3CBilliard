﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Entities
{
    public enum SlotStatus
    {
        ACTIVE,
        INACTIVE,
        WAITING,
        DELETED
    }

    [Table("Slot")]
    public class Slot
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public SlotStatus Status { get; set; }
    }

}
