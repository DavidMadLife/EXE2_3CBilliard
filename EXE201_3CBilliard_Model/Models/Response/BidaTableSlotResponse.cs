using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Response
{
    public class BidaTableSlotResponse
    {
        public long Id { get; set; }
        public long BidaTableId { get; set; }
        public long SlotId { get; set; }
        public TimeSpan SlotStartTime { get; set; }
        public TimeSpan SlotEndTime { get; set; }
        public string Status { get; set; }
    }
}

