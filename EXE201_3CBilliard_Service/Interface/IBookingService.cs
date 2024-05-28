using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingResponse>> GetAllBookingsAsync();
        Task<BookingResponse> GetBookingByIdAsync(long id);
        Task<BookingResponse> CreateBookingAsync(BookingRequest request);
        Task<BookingResponse> UpdateBookingAsync(long id, BookingRequest request);
        Task<bool> DeleteBookingAsync(long id);
        /*Task<BookingDetailResponse> GetBookingByOrderCodeAsync(string orderCode);*/
        Task<IEnumerable<BookingResponse>> BookMultipleSlotsAsync(long userId, List<long>? BT_SlotId);
    }
}
