using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using EXE201_3CBilliard_Repository.Repository;
using EXE201_3CBilliard_Service.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Service
{
    public class BillService : IBillService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly EXE201_3CBilliard_Repository.Tools.Firebase _firebase;

        public BillService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, EXE201_3CBilliard_Repository.Tools.Firebase firebase)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _firebase = firebase;
        }


        public async Task<BillResponse> GetAndSaveBillByOrderCodeAsync(BillRequest billRequest)
        {
            // Tính tổng giá cả của các đặt bàn có cùng orderCode
            var totalPrice = _unitOfWork.BookingRepository.Get()
                .Where(b => b.OrderCode == billRequest.OrderCode)
                .Sum(b => b.Price);

            // Lấy thông tin đặt bàn đầu tiên có cùng orderCode
            var firstBooking = _unitOfWork.BookingRepository.Get()
                .FirstOrDefault(b => b.OrderCode == billRequest.OrderCode);
            if (firstBooking == null)
                throw new Exception($"No booking found with order code {billRequest.OrderCode}");

            var bslot = _unitOfWork.BidaTableSlotRepository.GetById(firstBooking.BT_SlotId);

            var table = _unitOfWork.BidaTableRepository.GetById(bslot.BidaTableId);
                

            // Lấy danh sách các booking có cùng OrderCode 
            var bookingsToUpdate = _unitOfWork.BookingRepository.Get()
                .Where(b => b.OrderCode == billRequest.OrderCode)
                .ToList();

            // Danh sách để lưu trữ ID của các slot đã được đặt
            var bookedSlotIds = new List<long>();

            // Cập nhật trạng thái của các booking sang "ACTIVE" và thêm ID của slot vào danh sách
            foreach (var booking in bookingsToUpdate)
            {
                bookedSlotIds.Add(booking.BT_SlotId);
                _unitOfWork.BookingRepository.Update(booking);
            }

            // Lấy thông tin người dùng từ đặt bàn
            var user = _unitOfWork.UserRepository.GetById(firstBooking.UserId);
            if (user == null)
                throw new Exception($"User with id {firstBooking.UserId} not found.");

            // Sử dụng thông tin từ BillRequest nếu có, nếu không thì dùng thông tin từ user
            var bookerName = !string.IsNullOrEmpty(billRequest.BookerName) ? billRequest.BookerName : user.UserName;
            var bookerPhone = !string.IsNullOrEmpty(billRequest.BookerPhone) ? billRequest.BookerPhone : user.Phone;
            var bookerEmail = !string.IsNullOrEmpty(billRequest.BookerEmail) ? billRequest.BookerEmail : user.Email;

            var bill = new Bill
            {
                UserId = firstBooking.UserId,
                ClubId = table.BidaCludId,
                PaymentMethods = billRequest.PaymentMethods,
                BookerName = bookerName,
                BookerPhone = bookerPhone,
                BookerEmail = bookerEmail,
                Price = totalPrice,
                CreateAt = DateTime.Now,
                BookingDate = firstBooking.BookingDate.Date,
                OrderCode = billRequest.OrderCode,
                Descrpition = firstBooking.Descrpition,
                Status = BillStatus.WAITING
            };

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
                CreateAt = bill.CreateAt,
                BookingDate = firstBooking.BookingDate.Date,
                OrderCode = billRequest.OrderCode,
                Descrpition = bill.Descrpition,
                Status = BillStatus.WAITING.ToString(),
                BookedSlotIds = bookedSlotIds // Thêm danh sách ID của các slot đã được đặt vào BillResponse
            };

            return billResponse;
        }


        public async Task<BillResponse> UpdateBillStatusToActiveAsync(long billId)
        {
            var bill = _unitOfWork.BillRepository.GetById(billId);
            if (bill == null)
                throw new Exception($"Bill with id {billId} not found.");

            // Lấy danh sách các booking có cùng OrderCode và trạng thái là "WAITING"
            var bookingsToUpdate = _unitOfWork.BookingRepository.Get()
                .Where(b => b.OrderCode == bill.OrderCode && b.Status == BookingStatus.WAITING)
                .ToList();

            // Danh sách để lưu trữ ID của các slot đã được đặt
            var bookedSlotIds = new List<long>();

            // Cập nhật trạng thái của các booking sang "ACTIVE" và thêm ID của slot vào danh sách
            foreach (var booking in bookingsToUpdate)
            {
                long slots = booking.BT_SlotId;
                booking.Status = BookingStatus.ACTIVE;
                bookedSlotIds.Add(slots);
                _unitOfWork.BookingRepository.Update(booking);
            }

            // Cập nhật trạng thái của bill sang "ACTIVE"
            bill.Status = BillStatus.ACTIVE;
            _unitOfWork.BillRepository.Update(bill);

            // Lưu thay đổi vào cơ sở dữ liệu
            _unitOfWork.Save();

            var user = _unitOfWork.UserRepository.GetById(bill.UserId);
            if (user == null)
                throw new Exception($"User with id {bill.UserId} not found.");

            var billResponse = new BillResponse
            {
                Id = bill.Id,
                ClubId = bill.ClubId,
                BookerName = bill.BookerName,
                BookerPhone = bill.BookerPhone,
                BookerEmail = bill.BookerEmail,
                Price = bill.Price,
                CreateAt = bill.CreateAt,
                BookingDate = bill.BookingDate,
                OrderCode = bill.OrderCode,
                Descrpition = bill.Descrpition,
                Status = BillStatus.ACTIVE.ToString(),
                BookedSlotIds = bookedSlotIds // Thêm danh sách ID của các slot đã được đặt vào BillResponse
            };

            await _emailService.SendBillEmailAsync(user.Email, billResponse);

            var notification = new Notificate
            {
                Title = "Bill Activated",
                Descrpition = $"Your order with Order Code {bill.OrderCode} has been activated.",
                CreateAt = DateTime.Now,
                Status = NotificateStatus.ACTIVE,
                UserId = user.Id,
                Type = NotificationType.BookingNotification
            };

            _unitOfWork.NotificateRepository.Insert(notification);
            _unitOfWork.Save();

            return billResponse;
        }

        public async Task<BillResponse> UpdateBillStatusToInactiveAsync(long billId)
        {
            var bill = _unitOfWork.BillRepository.GetById(billId);
            if (bill == null)
                throw new Exception($"Bill with id {billId} not found.");

            // Lấy danh sách các booking có cùng OrderCode và trạng thái là "WAITING"
            var bookingsToUpdate = _unitOfWork.BookingRepository.Get()
                .Where(b => b.OrderCode == bill.OrderCode && b.Status == BookingStatus.WAITING)
                .ToList();

            // Danh sách để lưu trữ ID của các slot đã được đặt
            var bookedSlotIds = new List<long>();

            // Cập nhật trạng thái của các booking sang "INACTIVE" và thêm ID của slot vào danh sách
            foreach (var booking in bookingsToUpdate)
            {
                long slots = booking.BT_SlotId;
                booking.Status = BookingStatus.INACTIVE;
                bookedSlotIds.Add(slots);
                _unitOfWork.BookingRepository.Update(booking);
            }

            // Cập nhật trạng thái của bill sang "INACTIVE"
            bill.Status = BillStatus.INACTIVE;
            _unitOfWork.BillRepository.Update(bill);

            // Lưu thay đổi vào cơ sở dữ liệu
            _unitOfWork.Save();

            var user = _unitOfWork.UserRepository.GetById(bill.UserId);
            if (user == null)
                throw new Exception($"User with id {bill.UserId} not found.");

            var billResponse = new BillResponse
            {
                Id = bill.Id,
                ClubId = bill.ClubId,
                BookerName = bill.BookerName,
                BookerPhone = bill.BookerPhone,
                BookerEmail = bill.BookerEmail,
                Price = bill.Price,
                CreateAt = bill.CreateAt,
                BookingDate = bill.BookingDate,
                OrderCode = bill.OrderCode,
                Descrpition = bill.Descrpition,
                Status = BillStatus.INACTIVE.ToString(),
                BookedSlotIds = bookedSlotIds // Thêm danh sách ID của các slot đã được đặt vào BillResponse
            };

            await _emailService.SendRejectBookingEmailAsync(user.Email);

            var notification = new Notificate
            {
                Title = "Bill Rejected",
                Descrpition = $"Your bill with Order Code {bill.OrderCode} has been rejected.",
                CreateAt = DateTime.Now,
                Status = NotificateStatus.ACTIVE,
                UserId = user.Id,
                Type = NotificationType.BookingNotification
            };

            _unitOfWork.NotificateRepository.Insert(notification);
            _unitOfWork.Save();

            return billResponse;
        }

        public BillTotalResponse GetTotalAmountByDateRange(BillTotalRequest request)
        {
            var totalAmount = _unitOfWork.BillRepository.Get()
                .Where(b => b.Status == BillStatus.ACTIVE && b.CreateAt >= request.StartDate && b.CreateAt <= request.EndDate)
                .Sum(b => b.Price);

            return new BillTotalResponse
            {
                TotalAmount = totalAmount
            };
        }

        public async Task<(IEnumerable<BillResponse> bills, int totalCount)> SearchBillsAsync(long? userId, long? clubId, string? bookerName, DateTime? createAt, string? orderCode, string? status, int pageIndex, int pageSize)
        {
            // Khai báo biến statusEnum để lưu trữ giá trị enum BillStatus sau khi chuyển đổi
            BillStatus? statusEnum = null;

            // Thử chuyển đổi chuỗi status thành giá trị enum BillStatus
            if (!string.IsNullOrEmpty(status) && Enum.TryParse(status, true, out BillStatus parsedStatus))
            {
                statusEnum = parsedStatus;
            }

            // Áp dụng bộ lọc tìm kiếm
            var billsWithCount = _unitOfWork.BillRepository.GetWithCount(
                filter: x =>
                    (!userId.HasValue || x.UserId == userId.Value) &&
                    (!clubId.HasValue || x.ClubId == clubId.Value) &&
                    (string.IsNullOrEmpty(bookerName) || x.BookerName.Contains(bookerName)) &&
                    (!createAt.HasValue || x.CreateAt.Date == createAt.Value.Date) &&
                    (string.IsNullOrEmpty(orderCode) || x.OrderCode.Contains(orderCode)) &&
                    (!statusEnum.HasValue || x.Status == statusEnum.Value), // Sử dụng enum trực tiếp trong bộ lọc
                pageIndex: pageIndex,
                pageSize: pageSize
            );

            var bills = billsWithCount.items;
            var totalCount = billsWithCount.totalCount;

            // Create a list of BillResponse objects
            var billResponses = new List<BillResponse>();

            foreach (var bill in bills)
            {
                // Get the list of booked slot IDs for the current bill
                var bookedSlotIds = _unitOfWork.BookingRepository.Get(b => b.OrderCode == bill.OrderCode)
                                                                 .Select(b => b.BT_SlotId)
                                                                 .ToList();

                // Map the bill to a BillResponse object and include the booked slot IDs
                var billResponse = _mapper.Map<BillResponse>(bill);
                billResponse.BookedSlotIds = bookedSlotIds;

                billResponses.Add(billResponse);
            }

            return (billResponses, totalCount);
        }

        //noti
        public async Task<string> UpdateBillImageAsync(long billId, IFormFile img)
        {
            var bill = _unitOfWork.BillRepository.GetById(billId);
            if (bill == null)
            {
                throw new Exception($"Bill with id {billId} not found.");
            }

            if (img == null || img.Length == 0)
            {
                throw new Exception("No image file provided.");
            }

            if (img.Length >= 10 * 1024 * 1024)
            {
                throw new Exception("Image file size must be less than 10MB.");
            }

            string imageDownloadUrl = await _firebase.UploadImage(img);
            bill.Image = imageDownloadUrl;

            _unitOfWork.BillRepository.Update(bill);
            _unitOfWork.Save();

            var user = _unitOfWork.UserRepository.GetById(bill.UserId);
            if (user == null)
            {
                throw new Exception($"User with id {bill.UserId} not found.");
            }

            var notification = new Notificate
            {
                Title = "Bill Image Updated",
                Descrpition = $"The image for your bill with Order Code {bill.OrderCode} has been updated.",
                CreateAt = DateTime.Now,
                Status = NotificateStatus.ACTIVE,
                UserId = user.Id,
                Type = NotificationType.BookingNotification
            };

            _unitOfWork.NotificateRepository.Insert(notification);
            _unitOfWork.Save();

            return imageDownloadUrl;
        }

        public async Task CheckAndUpdateBillStatusAsync()
        {
            var bills = _unitOfWork.BillRepository.Get()
                .Where(b => b.Status == BillStatus.WAITING && b.CreateAt.AddHours(1) <= DateTime.Now)
                .ToList();

            var bookings = _unitOfWork.BookingRepository.Get()
                .Where(bk => bk.Status == BookingStatus.WAITING && bk.CreateAt.AddHours(1) <= DateTime.Now)
                .ToList();

            foreach (var booking in bookings)
            {
                booking.Status = BookingStatus.DELETED;
                _unitOfWork.BookingRepository.Update(booking);
                _unitOfWork.Save();
            }

            foreach (var bill in bills)
            {
                bill.Status = BillStatus.DELETED;
                _unitOfWork.BillRepository.Update(bill);
                _unitOfWork.Save();

                var user = _unitOfWork.UserRepository.GetById(bill.UserId);
                if (user != null)
                {
                    await _emailService.SendEmailAsync(user.Email, "Booking Failed", "Your booking has been cancelled due to inactivity.");
                }
            }
        }
    }
}
