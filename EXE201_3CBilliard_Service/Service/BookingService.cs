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
    }
}
