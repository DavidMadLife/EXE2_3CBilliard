using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using EXE201_3CBilliard_Repository.Repository;
using EXE201_3CBilliard_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Service
{
    public class BookingService :IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookingResponse>> GetAllBookingsAsync()
        {
            var bookings = _unitOfWork.BookingRepository.Get();
            return _mapper.Map<IEnumerable<BookingResponse>>(bookings);
        }

        public async Task<BookingResponse> GetBookingByIdAsync(long id)
        {
            var booking = _unitOfWork.BookingRepository.GetById(id);
            return _mapper.Map<BookingResponse>(booking);
        }

        public async Task<BookingResponse> CreateBookingAsync(BookingRequest request)
        {
            var booking = _mapper.Map<Booking>(request);
            _unitOfWork.BookingRepository.Insert(booking);
            _unitOfWork.Save();
            return _mapper.Map<BookingResponse>(booking);
        }

        public async Task<BookingResponse> UpdateBookingAsync(long id, BookingRequest request)
        {
            var booking = _unitOfWork.BookingRepository.GetById(id);
            if (booking == null)
                throw new Exception($"Booking with id {id} not found.");

            _mapper.Map(request, booking);
            _unitOfWork.BookingRepository.Update(booking);
            _unitOfWork.Save();
            return _mapper.Map<BookingResponse>(booking);
        }

        public async Task<bool> DeleteBookingAsync(long id)
        {
            var booking = _unitOfWork.BookingRepository.GetById(id);
            if (booking == null)
                throw new Exception($"Booking with id {id} not found.");

            _unitOfWork.BookingRepository.Delete(booking);
            _unitOfWork.Save();
            return true;
        }

        public async Task<IEnumerable<BookingResponse>> BookMultipleSlotsAsync(long userId, List<long>? BT_SlotId)
        {
            var user = _unitOfWork.UserRepository.GetById(userId);
            if (user == null)
                throw new Exception($"User with id {userId} not found.");

            var bookings = new List<Booking>();
            string code = GenerateRandomString();
            foreach (var slotId in BT_SlotId)
            {
                var slot = _unitOfWork.BidaTableSlotRepository.GetById(slotId);
                if (slot == null)
                    throw new Exception($"Slot with id {slotId} not found.");

                // Lấy thông tin về bảng BidaTable từ slot
                var bidaTable = _unitOfWork.BidaTableRepository.GetById(slot.BidaTableId);
                if (bidaTable == null)
                    throw new Exception($"BidaTable with id {slot.BidaTableId} not found.");

                var booking = new Booking
                {
                    BT_SlotId = slotId,
                    UserId = userId,
                    CreateAt = DateTime.Now,
                    OrderCode = code,
                    Descrpition = "THANH TOAN HOA DON 3CBILLIARD",
                    Note = "Note",
                    Status = BookingStatus.WAITING, // Assuming default status is WAITING
                    Price = bidaTable.Price // Assigning price from BidaTable
                };

                _unitOfWork.BookingRepository.Insert(booking);
                bookings.Add(booking);
            }

            _unitOfWork.Save();
            return _mapper.Map<IEnumerable<BookingResponse>>(bookings);
        }


        public string GenerateRandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string randomLetters = new string(Enumerable.Repeat(chars, 3)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            string randomNumber = random.Next(10000000, 99999999).ToString(); // Ensure 7 digits

            return randomLetters + randomNumber;
        }

        /*public async Task<BookingDetailResponse> GetBookingByOrderCodeAsync(string orderCode)
        {
            // Tính tổng giá cả của các đặt bàn có cùng orderCode
            var totalPrice = _unitOfWork.BookingRepository.Get()
                .Where(b => b.OrderCode == orderCode)
                .Sum(b => b.Price);

            // Lấy thông tin đặt bàn đầu tiên có cùng orderCode
            var firstBooking = _unitOfWork.BookingRepository.Get()
                .FirstOrDefault(b => b.OrderCode == orderCode);
            if (firstBooking == null)
                throw new Exception($"No booking found with order code {orderCode}");

            // Lấy thông tin người dùng từ đặt bàn
            var user = _unitOfWork.UserRepository.GetById(firstBooking.UserId);
            if (user == null)
                throw new Exception($"User with id {firstBooking.UserId} not found.");

            // Tạo response theo định dạng
            var response = new BookingDetailResponse
            {
                User = user.UserName, // Thay userId bằng tên người dùng
                CreateAt = firstBooking.CreateAt,
                OrderCode = orderCode,
                Descrpition = firstBooking.Descrpition,
                Price = totalPrice,
                Status = firstBooking.Status.ToString()
            };

            return response;
        }*/

        public async Task<(IEnumerable<BookingResponse> bookings, int TotalCount)> SearchBookingsAsync(long? userId, DateTime? createAt, string? orderCode, int pageIndex, int pageSize)
        {
            var result = _unitOfWork.BookingRepository.GetWithCount(
                filter: b => (userId == null || b.UserId == userId) &&
                             (createAt == null || b.CreateAt.Date == createAt.Value.Date) &&
                             (string.IsNullOrEmpty(orderCode) || b.OrderCode == orderCode),
                pageIndex: pageIndex,
                pageSize: pageSize
            );

            var bookings = result.items;
            var totalCount = result.totalCount;

            return (_mapper.Map<IEnumerable<BookingResponse>>(bookings), totalCount);
        }


    }
}
