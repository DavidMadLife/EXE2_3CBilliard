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
    public class StatisticsService : IStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatisticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public double CalculateDailyRevenue(long bidaClubId, DateTime date)
        {
            // Fetch all active bookings for the specified Bida Club and date
            var bookings = _unitOfWork.BookingRepository.Get(
                filter: b => b.BSlot.BidaTable.BidaCludId == bidaClubId
                          && b.CreateAt.Date == date.Date
                          && b.Status == BookingStatus.ACTIVE,
                includeProperties: "BSlot.BidaTable");

            // Calculate the total price
            double totalRevenue = bookings.Sum(b => b.Price);

            return totalRevenue;
        }

        public async Task<StatisticsResponse> GetStatisticsSummary()
        {
            var totalActiveClubs = _unitOfWork.BidaClubRepository.CountFilter(c => c.Status == BidaClubStatus.ACTIVE);
            var totalActiveUsers = _unitOfWork.UserRepository.CountFilter(u => u.Status == UserStatus.ACTIVE);
            var totalRevenue = _unitOfWork.BillRepository.Get(b => b.Status == BillStatus.ACTIVE)
                                                               .Sum(b => b.Price);
            var totalActiveTransactions = _unitOfWork.BillRepository.CountFilter(b => b.Status == BillStatus.ACTIVE);

            return new StatisticsResponse
            {
                TotalActiveClubs = totalActiveClubs,
                TotalActiveUsers = totalActiveUsers,
                TotalRevenue = totalRevenue,
                TotalActiveTransactions = totalActiveTransactions
            };
        }
    }
}
