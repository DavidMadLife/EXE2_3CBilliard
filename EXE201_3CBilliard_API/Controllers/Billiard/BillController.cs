using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.Billiard
{
    [Route("api/v1.0/bills")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillController(IBillService billService)
        {
            _billService = billService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAndSaveBillByOrderCode([FromBody] BillRequest billRequest)
        {
            
            try
            {
                var billResponse = await _billService.GetAndSaveBillByOrderCodeAsync(billRequest);
                return Ok(billResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("total-amount")]
        public async Task<ActionResult<BillTotalResponse>> GetTotalAmountByDateRange([FromBody] BillTotalRequest request)
        {
            var response = _billService.GetTotalAmountByDateRange(request);
            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBills([FromQuery] long? userId, [FromQuery] long? clubId, [FromQuery] string? bookerName, [FromQuery] DateTime? createAt, [FromQuery] string? orderCode, [FromQuery] string? status, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _billService.SearchBillsAsync(userId, clubId, bookerName, createAt, orderCode, status, pageIndex, pageSize);
            Response.Headers.Add("X-Total-Count", result.totalCount.ToString());
            return Ok(result.bills);
        }

        [HttpPut("activate/{billId}")]
        public async Task<IActionResult> UpdateBillStatusToActive(long billId)
        {
            try
            {
                var billResponse = await _billService.UpdateBillStatusToActiveAsync(billId);
                return Ok(billResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("reject/{billId}")]
        public async Task<IActionResult> UpdateBillStatusToInactive(long billId)
        {
            try
            {
                var billResponse = await _billService.UpdateBillStatusToInactiveAsync(billId);
                return Ok(billResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
