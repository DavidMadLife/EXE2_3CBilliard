using AutoMapper;
using EXE201_3CBilliard_Service.Interface;
using EXE201_3CBilliard_Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.Billiard
{
    [Route("api/v1.0/bidatableslots")]
    [ApiController]
    public class BidaTableSlotController : ControllerBase
    {

        private readonly IBidaTableSlotService _bidaTableSlotService;
        private readonly IMapper _mapper;

        public BidaTableSlotController(IBidaTableSlotService bidaTableSlotService, IMapper mapper)
        {
            _bidaTableSlotService = bidaTableSlotService;
            _mapper = mapper;
        }
        [HttpPost("{bidaTableId}/slots")]
        public async Task<IActionResult> AddSlotsToBidaTable(long bidaTableId, [FromBody] List<long> slotIds)
        {
            var result = await _bidaTableSlotService.AddSlotsToBidaTableAsync(bidaTableId, slotIds);
            return Ok(result);
        }

        [HttpGet("{bidaTableId}/slot-ids")]
        public async Task<IActionResult> GetSlotIdsByBidaTableId(long bidaTableId)
        {
            var result = await _bidaTableSlotService.GetSlotIdsByBidaTableIdAsync(bidaTableId);
            return Ok(result);
        }


    }
}
