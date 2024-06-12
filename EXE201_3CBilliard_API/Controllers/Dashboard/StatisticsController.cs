using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Service.Interface;
using EXE201_3CBilliard_Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.Dashboard
{
    [Route("api/v1.0/statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("{bidaClubId}/revenue")]
        public IActionResult GetDailyRevenue(long bidaClubId, [FromQuery] DateTime date)
        {
            try
            {
                var revenue = _statisticsService.CalculateDailyRevenue(bidaClubId, date);
                return Ok(new { BidaClubId = bidaClubId, Date = date, Revenue = revenue });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetStatisticsSummary()
        {
            StatisticsResponse response = await _statisticsService.GetStatisticsSummary();
            return Ok(response);
        }
    }
}
