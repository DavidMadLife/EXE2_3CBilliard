using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Response
{
    public class GetSlotByBidatableResponse
    {
        public long BidaTableId { get; set; }
        public List<SlotResponse> SlotIds { get; set; }
    }
}
