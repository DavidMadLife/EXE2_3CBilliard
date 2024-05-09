using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Respone;
using EXE201_3CBilliard_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.Billiard
{
    [Route("api/[controller]")]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidaClubReponse>>> GetAll()
        {
            var bidaClubs = await _bidaClubService.GetAllBidaClubsAsync();
            return Ok(bidaClubs);
        }

        [HttpPost]
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

        [HttpDelete("{id}")]
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
    }
}
