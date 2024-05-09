using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface ISlotService
    {
        Task<IEnumerable<SlotResponse>> GetAllSlotsAsync();
        Task<SlotResponse> GetSlotByIdAsync(long id);
        Task<SlotResponse> CreateSlotAsync(SlotRequest request);
        Task<SlotResponse> UpdateSlotAsync(long id, SlotRequest request);
        Task<bool> DeleteSlotAsync(long id);
    }
}
