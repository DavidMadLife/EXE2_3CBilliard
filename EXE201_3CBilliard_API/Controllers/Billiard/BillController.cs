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

        [HttpPost("{orderCode}")]
        public async Task<IActionResult> SaveBookingByOrderCode(string orderCode)
        {
            var bill = await _billService.GetAndSaveBillByOrderCodeAsync(orderCode);
            return Ok(bill);
        }
        [HttpPut("activate/{billId}")]
        public async Task<IActionResult> ActivateBill(long billId)
        {
            var billResponse = await _billService.UpdateBillStatusToActiveAsync(billId);
            return Ok(billResponse);
        }
    }
}
