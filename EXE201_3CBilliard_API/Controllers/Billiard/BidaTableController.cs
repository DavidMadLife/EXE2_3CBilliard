using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Service.Interface;
using EXE201_3CBilliard_Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.Billiard
{
    [Route("api/v1.0/bidatables")]
    [ApiController]
    public class BidaTableController : ControllerBase
    {
        private readonly IBidaTableService _bidaTableService;

        public BidaTableController(IBidaTableService bidaTableService)
        {
            _bidaTableService = bidaTableService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<BidaTableResponse>>> GetAll()
        {
            var bidaTables = await _bidaTableService.GetAllBidaTablesAsync();
            return Ok(bidaTables);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BidaTableResponse>> GetById(long id)
        {
            var bidaTable = await _bidaTableService.GetBidaTableByIdAsync(id);
            if (bidaTable == null)
                return NotFound();

            return Ok(bidaTable);
        }

        [HttpPost("create")]
        public async Task<ActionResult<BidaTableResponse>> Create([FromBody] BidaTableRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdBidaTable = await _bidaTableService.CreateBidaTableAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdBidaTable.Id }, createdBidaTable);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BidaTableResponse>> Update(long id, [FromBody] BidaTableRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedBidaTable = await _bidaTableService.UpdateBidaTableAsync(id, request);
                return Ok(updatedBidaTable);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("delete/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _bidaTableService.DeleteBidaTableAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("inactive/{id}")]
        public async Task<ActionResult> Inactive(long id)
        {
            try
            {
                await _bidaTableService.InactiveBidaTableAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBidaTables(string? tableName, double? price, long? bidaClubId, int pageIndex = 1, int pageSize = 10)
        {
            var result = await _bidaTableService.SearchBidaTablesAsync(tableName, price, bidaClubId, pageIndex, pageSize);
            Response.Headers.Add("X-Total-Count", result.totalCount.ToString());
            return Ok(result.bidaTables);
        }


    }
}
