using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using EXE201_3CBilliard_Repository.Repository;
using EXE201_3CBilliard_Repository.Tools;
using EXE201_3CBilliard_Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        private readonly EXE201_3CBilliard_Repository.Tools.Firebase _firebase;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, EXE201_3CBilliard_Repository.Tools.Firebase firebase)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebase = firebase;
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


        //Cũ
        public async Task<IEnumerable<BookingResponse>> BookMultipleSlotsAsync(long userId, List<long>? BT_SlotId, DateTime bookingDate)
        {
            var currentDate = DateTime.Now; // Lấy ngày hiện tại
            var maxDate = currentDate.AddDays(7); // Ngày tối đa là 7 ngày sau

            if (bookingDate.Date > maxDate)
            {
                throw new Exception("Ngày đặt chỗ phải nhỏ hơn 7 ngày sau.");
            }

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

                var bidaTable = _unitOfWork.BidaTableRepository.GetById(slot.BidaTableId);
                if (bidaTable == null)
                    throw new Exception($"BidaTable with id {slot.BidaTableId} not found.");

                // Check if the slot is already booked for the given date
                var existingBooking = _unitOfWork.BookingRepository.Get()
                    .FirstOrDefault(b => b.BT_SlotId == slotId && b.BookingDate.Date == bookingDate.Date && (b.Status == BookingStatus.ACTIVE || b.Status == BookingStatus.WAITING));

                if (existingBooking != null)
                {
                    throw new Exception($"Slot with id {slotId} is already booked for {bookingDate.ToShortDateString()}.");
                }

                // Check if the booking is at least 1 hours in advance
                var slotTime = _unitOfWork.SlotRepository.GetById(slot.SlotId);
                if (slotTime == null)
                    throw new Exception($"{slotTime} not found.");
                var slotStartTime = bookingDate.Date.Add(slotTime.StartTime);
                if (slotStartTime <= currentDate.AddHours(1))
                {
                    throw new Exception($"Booking for slot with id {slotId} must be made at least 1 hours in advance.");
                }

                var booking = new Booking
                {
                    BT_SlotId = slotId,
                    UserId = userId,
                    CreateAt = DateTime.Now,
                    BookingDate = bookingDate.Date, // Use the provided booking date
                    OrderCode = code,
                    Descrpition = "THANH TOAN HOA DON 3CBILLIARD",
                    Note = "Note",
                    Status = BookingStatus.WAITING,
                    Price = bidaTable.Price
                };

                _unitOfWork.BookingRepository.Insert(booking);
                bookings.Add(booking);
            }

            _unitOfWork.Save();
            return _mapper.Map<IEnumerable<BookingResponse>>(bookings);
        }


        //Xài
        public async Task<IEnumerable<BookingResponse>> ClubOwnerBookSlotsAsync(long userId, List<long>? BT_SlotId, DateTime bookingDate)
        {
            var currentDate = DateTime.Now; // Lấy ngày hiện tại
            var maxDate = currentDate.AddDays(7); // Ngày tối đa là 7 ngày sau

            if (bookingDate.Date > maxDate)
            {
                throw new Exception("Ngày đặt chỗ phải nhỏ hơn 7 ngày sau.");
            }

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

                var bidaTable = _unitOfWork.BidaTableRepository.GetById(slot.BidaTableId);
                if (bidaTable == null)
                    throw new Exception($"BidaTable with id {slot.BidaTableId} not found.");

                // Check if the slot is already booked for the given date
                var existingBooking = _unitOfWork.BookingRepository.Get()
                    .FirstOrDefault(b => b.BT_SlotId == slotId && b.BookingDate.Date == bookingDate.Date && (b.Status == BookingStatus.ACTIVE || b.Status == BookingStatus.WAITING));

                if (existingBooking != null)
                {
                    throw new Exception($"Slot with id {slotId} is already booked for {bookingDate.ToShortDateString()}.");
                }

                //Club Owner don't check time at least 1 hours
                var slotTime = _unitOfWork.SlotRepository.GetById(slot.SlotId);
                if (slotTime == null)
                    throw new Exception($"{slotTime} not found.");
                var slotStartTime = bookingDate.Date.Add(slotTime.StartTime);
                if (slotStartTime <= currentDate)
                {
                    throw new Exception($"Booking for slot with id {slotId} must be made at least 1 hours in advance.");
                }

                var booking = new Booking
                {
                    BT_SlotId = slotId,
                    UserId = userId,
                    CreateAt = DateTime.Now,
                    BookingDate = bookingDate.Date, // Use the provided booking date
                    OrderCode = code,
                    Descrpition = "CLUB OWNER BOOK SLOT",
                    Note = "Note",
                    Status = BookingStatus.ACTIVE,
                    Price = bidaTable.Price
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

        //Cũ
        public async Task<BillResponse> BookSlotsAndGenerateBillAsync(long userId, List<long> BT_SlotIds, DateTime bookingDate, BillRequest billRequest, IFormFile img)
        {
            var currentDate = DateTime.Now;
            var maxDate = currentDate.AddDays(7);

            if (bookingDate.Date > maxDate)
            {
                throw new Exception("Ngày đặt chỗ phải nhỏ hơn 7 ngày sau.");
            }

            var user = _unitOfWork.UserRepository.GetById(userId);
            if (user == null)
                throw new Exception($"User with id {userId} not found.");

            var bookings = new List<Booking>();
            string orderCode = GenerateRandomString();
            long tableId = 0;

            foreach (var slotId in BT_SlotIds)
            {
                var slot = _unitOfWork.BidaTableSlotRepository.GetById(slotId);
                if (slot == null)
                    throw new Exception($"Slot with id {slotId} not found.");
                tableId = slot.BidaTableId;
                var bidaTable = _unitOfWork.BidaTableRepository.GetById(slot.BidaTableId);
                if (bidaTable == null)
                    throw new Exception($"BidaTable with id {slot.BidaTableId} not found.");

                var existingBooking = _unitOfWork.BookingRepository.Get()
                    .FirstOrDefault(b => b.BT_SlotId == slotId && b.BookingDate.Date == bookingDate.Date && (b.Status == BookingStatus.ACTIVE || b.Status == BookingStatus.WAITING));

                if (existingBooking != null)
                {
                    throw new Exception($"Slot with id {slotId} is already booked for {bookingDate.ToShortDateString()}.");
                }

                var slotTime = _unitOfWork.SlotRepository.GetById(slot.SlotId);
                if (slotTime == null)
                    throw new Exception($"{slotTime} not found.");
                var slotStartTime = bookingDate.Date.Add(slotTime.StartTime);
                if (slotStartTime <= currentDate.AddHours(1))
                {
                    throw new Exception($"Booking for slot with id {slotId} must be made at least 1 hour in advance.");
                }

                var booking = new Booking
                {
                    BT_SlotId = slotId,
                    UserId = userId,
                    CreateAt = DateTime.Now,
                    BookingDate = bookingDate.Date,
                    OrderCode = orderCode,
                    Descrpition = "THANH TOAN HOA DON 3CBILLIARD",
                    Note = "Note",
                    Status = BookingStatus.WAITING,
                    Price = bidaTable.Price
                };

                _unitOfWork.BookingRepository.Insert(booking);
                bookings.Add(booking);
            }

            _unitOfWork.Save();

            var totalPrice = bookings.Sum(b => b.Price);
            var firstBooking = bookings.FirstOrDefault();
            if (firstBooking == null)
                throw new Exception("No bookings found to generate bill.");

            var bookedSlotIds = bookings.Select(b => b.BT_SlotId).ToList();

            var bookerName = !string.IsNullOrEmpty(billRequest.BookerName) ? billRequest.BookerName : user.UserName;
            var bookerPhone = !string.IsNullOrEmpty(billRequest.BookerPhone) ? billRequest.BookerPhone : user.Phone;
            var bookerEmail = !string.IsNullOrEmpty(billRequest.BookerEmail) ? billRequest.BookerEmail : user.Email;

            var bibaTable = _unitOfWork.BidaTableRepository.Get()
                .FirstOrDefault(bc => bc.Id == tableId && bc.Status == BidaTableStatus.ACTIVE);

            var bill = new Bill
            {
                UserId = firstBooking.UserId,
                ClubId = bibaTable.BidaCludId,
                PaymentMethods = billRequest.PaymentMethods,
                BookerName = bookerName,
                BookerPhone = bookerPhone,
                BookerEmail = bookerEmail,
                Price = totalPrice,
                CreateAt = DateTime.Now,
                BookingDate = firstBooking.BookingDate.Date,
                OrderCode = orderCode,
                Descrpition = firstBooking.Descrpition,
                Status = BillStatus.WAITING
            };

            if (img != null)
            {
                if (img.Length >= 10 * 1024 * 1024)
                {
                    throw new Exception();
                }
                string imageDownloadUrl = await _firebase.UploadImage(img);
                bill.Image = imageDownloadUrl;
            }
            _unitOfWork.BillRepository.Insert(bill);
            _unitOfWork.Save();

            var billResponse = new BillResponse
            {
                Id = bill.Id,
                ClubId = bill.ClubId,
                BookerName = bookerName,
                BookerPhone = bookerPhone,
                BookerEmail = bookerEmail,
                Price = totalPrice,
                Image = bill.Image,
                CreateAt = bill.CreateAt,
                BookingDate = firstBooking.BookingDate.Date,
                OrderCode = orderCode,
                Descrpition = bill.Descrpition,
                Status = BillStatus.WAITING.ToString(),
                BookedSlotIds = bookedSlotIds
            };

            return billResponse;
        }

        public async Task<(IEnumerable<BookingResponse> bookings, int TotalCount)> SearchBookingsAsync(long? userId, DateTime? createAt, DateTime? bookingDate, string? orderCode, string? status, int pageIndex, int pageSize)
        {
            // Khai báo biến statusEnum để lưu trữ giá trị enum BookingStatus sau khi chuyển đổi
            BookingStatus? statusEnum = null;

            // Thử chuyển đổi chuỗi status thành giá trị enum BookingStatus
            if (!string.IsNullOrEmpty(status) && Enum.TryParse(status, true, out BookingStatus parsedStatus))
            {
                statusEnum = parsedStatus;
            }

            var result = _unitOfWork.BookingRepository.GetWithCount(
                filter: b => (userId == null || b.UserId == userId) &&
                             (createAt == null || b.CreateAt.Date == createAt.Value.Date) &&
                             (bookingDate == null || b.BookingDate.Date == bookingDate.Value.Date) &&
                             (string.IsNullOrEmpty(orderCode) || b.OrderCode == orderCode) &&
                            (!statusEnum.HasValue || b.Status == statusEnum.Value),
                pageIndex: pageIndex,
                pageSize: pageSize
            );

            var bookings = result.items;
            var totalCount = result.totalCount;

            return (_mapper.Map<IEnumerable<BookingResponse>>(bookings), totalCount);
        }


        public async Task<BillResponse> BookSlotsAndGenerateBillAsyncv2(CombinedBookAndBillRequest request)
        {
            var userId = request.UserId;
            var BT_SlotIds = request.BT_SlotIds;
            var bookingDate = request.BookingDate;
            var img = request.Image;

            var currentDate = DateTime.Now;
            var maxDate = currentDate.AddDays(7);

            if (bookingDate.Date > maxDate)
            {
                throw new Exception("Ngày đặt chỗ phải nhỏ hơn 7 ngày sau.");
            }

            var user = _unitOfWork.UserRepository.GetById(userId);
            if (user == null)
                throw new Exception($"User with id {userId} not found.");

            var bookings = new List<Booking>();
            string orderCode = request.OrderCode;
            long tableId = 0;

            foreach (var slotId in BT_SlotIds)
            {
                var slot = _unitOfWork.BidaTableSlotRepository.GetById(slotId);
                if (slot == null)
                    throw new Exception($"Slot with id {slotId} not found.");
                tableId = slot.BidaTableId;
                var bidaTable = _unitOfWork.BidaTableRepository.GetById(slot.BidaTableId);
                if (bidaTable == null)
                    throw new Exception($"BidaTable with id {slot.BidaTableId} not found.");

                var existingBooking = _unitOfWork.BookingRepository.Get()
                    .FirstOrDefault(b => b.BT_SlotId == slotId && b.BookingDate.Date == bookingDate.Date && (b.Status == BookingStatus.ACTIVE || b.Status == BookingStatus.WAITING));

                if (existingBooking != null)
                {
                    throw new Exception($"Slot with id {slotId} is already booked for {bookingDate.ToShortDateString()}.");
                }

                var slotTime = _unitOfWork.SlotRepository.GetById(slot.SlotId);
                if (slotTime == null)
                    throw new Exception($"{slotTime} not found.");
                var slotStartTime = bookingDate.Date.Add(slotTime.StartTime);
                if (slotStartTime <= currentDate.AddHours(1))
                {
                    throw new Exception($"Booking for slot with id {slotId} must be made at least 1 hour in advance.");
                }

                var booking = new Booking
                {
                    BT_SlotId = slotId,
                    UserId = userId,
                    CreateAt = DateTime.Now,
                    BookingDate = bookingDate.Date,
                    OrderCode = orderCode,
                    Descrpition = "THANH TOAN HOA DON 3CBILLIARD",
                    Note = "Note",
                    Status = BookingStatus.WAITING,
                    Price = bidaTable.Price
                };

                _unitOfWork.BookingRepository.Insert(booking);
                bookings.Add(booking);
            }

            _unitOfWork.Save();

            var totalPrice = bookings.Sum(b => b.Price);
            var firstBooking = bookings.FirstOrDefault();
            if (firstBooking == null)
                throw new Exception("No bookings found to generate bill.");

            var bookedSlotIds = bookings.Select(b => b.BT_SlotId).ToList();

            var bookerName = !string.IsNullOrEmpty(request.BookerName) ? request.BookerName : user.UserName;
            var bookerPhone = !string.IsNullOrEmpty(request.BookerPhone) ? request.BookerPhone : user.Phone;
            var bookerEmail = !string.IsNullOrEmpty(request.BookerEmail) ? request.BookerEmail : user.Email;

            var bibaTable = _unitOfWork.BidaTableRepository.Get()
                .FirstOrDefault(bc => bc.Id == tableId && bc.Status == BidaTableStatus.ACTIVE);

            var bill = new Bill
            {
                UserId = firstBooking.UserId,
                ClubId = bibaTable.BidaCludId,
                PaymentMethods = request.PaymentMethods,
                BookerName = bookerName,
                BookerPhone = bookerPhone,
                BookerEmail = bookerEmail,
                Price = totalPrice,
                CreateAt = DateTime.Now,
                BookingDate = firstBooking.BookingDate.Date,
                OrderCode = orderCode,
                Descrpition = firstBooking.Descrpition,
                Status = BillStatus.WAITING
            };

            if (img == null)
            {
                string imgDefault = "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png";
                bill.Image = imgDefault;
            }

            if (img != null)
            {
                if (img.Length >= 10 * 1024 * 1024)
                {
                    throw new Exception();
                }
                string imageDownloadUrl = await _firebase.UploadImage(img);
                bill.Image = imageDownloadUrl;
            }
            _unitOfWork.BillRepository.Insert(bill);
            _unitOfWork.Save();

            var billResponse = new BillResponse
            {
                Id = bill.Id,
                ClubId = bill.ClubId,
                BookerName = bookerName,
                BookerPhone = bookerPhone,
                BookerEmail = bookerEmail,
                Price = totalPrice,
                Image = bill.Image,
                CreateAt = bill.CreateAt,
                BookingDate = firstBooking.BookingDate.Date,
                OrderCode = orderCode,
                Descrpition = bill.Descrpition,
                Status = BillStatus.WAITING.ToString(),
                BookedSlotIds = bookedSlotIds
            };

            return billResponse;
        }

    }
}
