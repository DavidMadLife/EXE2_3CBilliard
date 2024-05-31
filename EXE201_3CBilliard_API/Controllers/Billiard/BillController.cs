using EXE201_3CBilliard_Model.Models.Request;
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
    }
}
