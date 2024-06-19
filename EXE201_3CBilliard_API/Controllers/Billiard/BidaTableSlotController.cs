using AutoMapper;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Service.Exceptions;
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
       /* [HttpPost("{bidaTableId:long}/slots")]
        public async Task<IActionResult> AddSlotsToBidaTable(long bidaTableId, [FromBody] List<long> slotIds)
        {
            try
            {
                var response = await _bidaTableSlotService.AddSlotsToBidaTableAsync(bidaTableId, slotIds);
                return Ok(response);
            }
            catch (SlotAlreadyExistsException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }*/

        [HttpGet("{bidaTableId}/slot-ids")]
        public async Task<IActionResult> GetSlotIdsByBidaTableId(long bidaTableId)
        {
            var result = await _bidaTableSlotService.GetSlotIdsByBidaTableIdAsync(bidaTableId);
            return Ok(result);
        }

        [HttpPut("{bidaTableId:long}/slots")]
        public async Task<IActionResult> UpdateSlotsOfBidaTable(long bidaTableId, [FromBody] List<long> slotIds)
        {
            try
            {
                var response = await _bidaTableSlotService.UpdateSlotsOfBidaTableAsync(bidaTableId, slotIds);
                return Ok(response);
            }
            catch (SlotAlreadyExistsException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{bidaTableId}/delete")]
        public async Task<IActionResult> DeleteBidaTableAndSlots(long bidaTableId)
        {
            try
            {
                await _bidaTableSlotService.DeleteBidaTableAndSlotsAsync(bidaTableId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBidaTableSlot()
        {
            var bidaTableSlots =  await _bidaTableSlotService.GetAllAsync();
            return Ok(bidaTableSlots);
        }


        [HttpGet("booked-slots")]
        public async Task<IActionResult> GetBookedSlotsByDateAndTable([FromQuery] DateTime bookingDate, [FromQuery] long bidaTableId)
        {
            try
            {
                var bookedSlots = await _bidaTableSlotService.GetBookedSlotsByDateAndTableAsync(bookingDate, bidaTableId);
                if (bookedSlots == null || !bookedSlots.Any())
                {
                    return NotFound("No booked slots found for the specified date and table.");
                }

                return Ok(bookedSlots);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet("bidaTableId")]
        public async Task<ActionResult<IEnumerable<BidaTableSlotResponse>>> GetBidaTableSlots([FromQuery] long? bidaTableId, [FromQuery] long? slotId)
        {
            var result = await _bidaTableSlotService.GetBidaTableSlotsAsync(bidaTableId, slotId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBidaTableSlotById(long id)
        {
            try
            {
                var bidaTableSlot = await _bidaTableSlotService.GetByIdAsync(id);
                return Ok(bidaTableSlot);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


    }
}
