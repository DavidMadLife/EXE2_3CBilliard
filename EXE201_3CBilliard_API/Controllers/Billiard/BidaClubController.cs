using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Respone;
using EXE201_3CBilliard_Service.Interface;
using EXE201_3CBilliard_Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.Billiard
{
    [Route("api/v1.0/bidaclubs")]
    [ApiController]
    public class BidaClubController : ControllerBase
    {
        private readonly IBidaClubService _bidaClubService;
        private readonly IMapper _mapper;

        public BidaClubController(IBidaClubService bidaClubService, IMapper mapper)
        {
            _bidaClubService = bidaClubService;
            _mapper = mapper;
        }

        [HttpPut("activate/{id}")]
        public async Task<IActionResult> Activate(long id, [FromBody] NoteRequest noteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _bidaClubService.ActivateBidaClubAsync(id, noteRequest);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reject/{id}")]
        public async Task<IActionResult> Reject(long id, [FromBody] NoteRequest noteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _bidaClubService.RejectBidaClubAsync(id, noteRequest);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BidaClubReponse>> Get(long id)
        {
            var bidaClub = await _bidaClubService.GetBidaClubByIdAsync(id);
            if (bidaClub == null)
            {
                return NotFound();
            }
            return Ok(bidaClub);
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<BidaClubReponse>>> GetAll()
        {
            var bidaClubs = await _bidaClubService.GetAllBidaClubsAsync();
            return Ok(bidaClubs);
        }

        [HttpPost("create")]
        public async Task<ActionResult<BidaClubReponse>> Create(BidaClubRequest request)
        {
            var createdBidaClub = await _bidaClubService.CreateBidaClubAsync(request);
            return CreatedAtAction(nameof(Get), new { id = createdBidaClub.Id }, createdBidaClub);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BidaClubReponse>> Update(long id, BidaClubRequest request)
        {
            try
            {
                var updatedBidaClub = await _bidaClubService.UpdateBidaClubAsync(id, request);
                return Ok(updatedBidaClub);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _bidaClubService.DeleteBidaClubAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBidaClubs([FromQuery] string? bidaName, [FromQuery] string? address, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _bidaClubService.SearchBidaClubsAsync(bidaName, address, pageIndex, pageSize);
            Response.Headers.Add("X-Total-Count", result.totalCount.ToString());
            return Ok(result.bidaClubs);
        }

    }
}
