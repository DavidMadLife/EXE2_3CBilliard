﻿using AutoMapper;
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
    public class BillService : IBillService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public BillService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
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
                PaymentMethods = billRequest.PaymentMethods,
                BookerName = bookerName,
                BookerPhone = bookerPhone,
                BookerEmail = bookerEmail,
                Price = totalPrice,
                CreateAt = DateTime.Now,
                OrderCode = billRequest.OrderCode,
                Descrpition = firstBooking.Descrpition,
                Status = BillStatus.WAITING
            };

            _unitOfWork.BillRepository.Insert(bill);
            _unitOfWork.Save();

            var billResponse = new BillResponse
            {
                Id = bill.Id,
                BookerName = bookerName,
                BookerPhone = bookerPhone,
                BookerEmail = bookerEmail,
                Price = totalPrice,
                CreateAt = bill.CreateAt,
                OrderCode = billRequest.OrderCode,
                Descrpition = bill.Descrpition,
                Status = BillStatus.WAITING.ToString()
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

            // Cập nhật trạng thái của các booking sang "ACTIVE"
            foreach (var booking in bookingsToUpdate)
            {
                booking.Status = BookingStatus.ACTIVE;
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
                BookerName = bill.BookerName,
                BookerPhone = bill.BookerPhone,
                BookerEmail = bill.BookerEmail,
                Price = bill.Price,
                CreateAt = bill.CreateAt,
                OrderCode = bill.OrderCode,
                Descrpition = bill.Descrpition,
                Status = BillStatus.ACTIVE.ToString()
            };

            await _emailService.SendBillEmailAsync(user.Email, billResponse);

            return billResponse;
        }


        public async Task CheckAndUpdateBillStatusAsync()
        {
            var bills = _unitOfWork.BillRepository.Get()
                .Where(b => b.Status == BillStatus.WAITING && b.CreateAt.AddHours(1) <= DateTime.Now)
                .ToList();

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
