using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface IBidaTableSlotService
    {
        /*Task<IEnumerable<BidaTableSlotResponse>> AddSlotsToBidaTableAsync(long bidaTableId, List<long>? slotIds);*/
        Task<GetSlotByBidatableResponse> GetSlotIdsByBidaTableIdAsync(long bidaTableId);
        Task<GetSlotByBidatableResponse> UpdateSlotsOfBidaTableAsync(long bidaTableId, List<long> slotIds);
        Task DeleteBidaTableAndSlotsAsync(long bidaTableId);
        Task<IEnumerable<BidaTableSlotResponse>> GetAllAsync();

        Task<IEnumerable<BidaTableSlotResponse>> GetBookedSlotsByDateAndTableAsync(DateTime bookingDate, long bidaTableId);

        Task<IEnumerable<BidaTableSlotResponse>> GetBidaTableSlotsAsync(long? bidaTableId, long? slotId);

        Task<BidaTableSlotResponse> GetByIdAsync(long bidaTableSlotId);


    }
}

