using EXE201_3CBilliard_Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Interface
{
    public interface IStatisticsService
    {
        double CalculateDailyRevenue(long clubId, DateTime date);
        Task<StatisticsResponse> GetStatisticsSummary();
        Task<ListStatisticsResponse<DailyStatisticsResponse>> GetDailyStatistics(DateTime startDate, DateTime endDate);
        Task<ListStatisticsResponse<MonthlyStatisticsResponse>> GetMonthlyStatistics(DateTime startDate, DateTime endDate);
        Task<ListStatisticsResponse<YearlyStatisticsResponse>> GetYearlyStatistics(DateTime startDate, DateTime endDate);
    }
}
