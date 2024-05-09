using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.Billiard
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidaTableController : ControllerBase
    {
        private readonly IBidaTableService _bidaTableService;

        public BidaTableController(IBidaTableService bidaTableService)
        {
            _bidaTableService = bidaTableService;
        }

        [HttpGet]
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

        [HttpPost]
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var result = await _bidaTableService.DeleteBidaTableAsync(id);
                if (result)
                    return NoContent();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
