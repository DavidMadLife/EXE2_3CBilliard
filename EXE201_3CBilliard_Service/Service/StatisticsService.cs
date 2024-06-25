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

        public async Task<ListStatisticsResponse<DailyStatisticsResponse>> GetDailyStatistics(DateTime startDate, DateTime endDate)
        {
            var dailyStatistics = new List<DailyStatisticsResponse>();

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var userRegistrations = _unitOfWork.UserRepository.CountFilter(u => u.CreateAt.Date == date);
                var bidaClubRegistrations = _unitOfWork.BidaClubRepository.CountFilter(c => c.CreateAt.Date == date);
                var transactions = _unitOfWork.BillRepository.CountFilter(b => b.CreateAt.Date == date && b.Status == BillStatus.ACTIVE);
                var totalRevenue = _unitOfWork.BillRepository.Get(b => b.CreateAt.Date == date && b.Status == BillStatus.ACTIVE).Sum(b => b.Price);

                dailyStatistics.Add(new DailyStatisticsResponse
                {
                    Date = date,
                    UserRegistrations = userRegistrations,
                    BidaClubRegistrations = bidaClubRegistrations,
                    Transactions = transactions,
                    TotalRevenue = totalRevenue
                });
            }

            return new ListStatisticsResponse<DailyStatisticsResponse>
            {
                Statistics = dailyStatistics
            };
        }

        public async Task<ListStatisticsResponse<MonthlyStatisticsResponse>> GetMonthlyStatistics(DateTime startDate, DateTime endDate)
        {
            var monthlyStatistics = new List<MonthlyStatisticsResponse>();

            var groupedByMonth = _unitOfWork.UserRepository.Get()
                .Where(u => u.CreateAt.Date >= startDate.Date && u.CreateAt.Date <= endDate.Date)
                .GroupBy(u => new { u.CreateAt.Year, u.CreateAt.Month });

            foreach (var group in groupedByMonth)
            {
                var userRegistrations = group.Count();
                var bidaClubRegistrations = _unitOfWork.BidaClubRepository.CountFilter(c => c.CreateAt.Year == group.Key.Year && c.CreateAt.Month == group.Key.Month);
                var transactions = _unitOfWork.BillRepository.CountFilter(b => b.CreateAt.Year == group.Key.Year && b.CreateAt.Month == group.Key.Month && b.Status == BillStatus.ACTIVE);
                var totalRevenue = _unitOfWork.BillRepository.Get(b => b.CreateAt.Year == group.Key.Year && b.CreateAt.Month == group.Key.Month && b.Status == BillStatus.ACTIVE).Sum(b => b.Price);

                monthlyStatistics.Add(new MonthlyStatisticsResponse
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    UserRegistrations = userRegistrations,
                    BidaClubRegistrations = bidaClubRegistrations,
                    Transactions = transactions,
                    TotalRevenue = totalRevenue
                });
            }

            return new ListStatisticsResponse<MonthlyStatisticsResponse>
            {
                Statistics = monthlyStatistics
            };
        }

        public async Task<ListStatisticsResponse<YearlyStatisticsResponse>> GetYearlyStatistics(DateTime startDate, DateTime endDate)
        {
            var yearlyStatistics = new List<YearlyStatisticsResponse>();

            var groupedByYear = _unitOfWork.UserRepository.Get()
                .Where(u => u.CreateAt.Date >= startDate.Date && u.CreateAt.Date <= endDate.Date)
                .GroupBy(u => u.CreateAt.Year);

            foreach (var group in groupedByYear)
            {
                var userRegistrations = group.Count();
                var bidaClubRegistrations = _unitOfWork.BidaClubRepository.CountFilter(c => c.CreateAt.Year == group.Key);
                var transactions = _unitOfWork.BillRepository.CountFilter(b => b.CreateAt.Year == group.Key && b.Status == BillStatus.ACTIVE);
                var totalRevenue = _unitOfWork.BillRepository.Get(b => b.CreateAt.Year == group.Key && b.Status == BillStatus.ACTIVE).Sum(b => b.Price);

                yearlyStatistics.Add(new YearlyStatisticsResponse
                {
                    Year = group.Key,
                    UserRegistrations = userRegistrations,
                    BidaClubRegistrations = bidaClubRegistrations,
                    Transactions = transactions,
                    TotalRevenue = totalRevenue
                });
            }

            return new ListStatisticsResponse<YearlyStatisticsResponse>
            {
                Statistics = yearlyStatistics
            };
        }
    }
}
