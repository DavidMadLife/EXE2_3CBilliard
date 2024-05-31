﻿using AutoMapper;
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

        [HttpPut("{bidaTableId}/update-slots")]
        public async Task<IActionResult> UpdateSlotsOfBidaTable(long bidaTableId, [FromBody] List<long> slotIds)
        {
            if (slotIds == null || !slotIds.Any())
                return BadRequest("SlotIds cannot be null or empty.");

            var result = await _bidaTableSlotService.UpdateSlotsOfBidaTableAsync(bidaTableId, slotIds);
            return Ok(result);
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


        [HttpGet("{bidaTableId}")]
        public async Task<IActionResult> GetBidaTableSlotById(long bidaTableId)
        {
            try
            {
                var bidaTableSlot = await _bidaTableSlotService.GetBidaTableSlotByIdAsync(bidaTableId);
                if (bidaTableSlot == null)
                {
                    return NotFound($"BidaTableSlot with id {bidaTableId} not found.");
                }
                return Ok(bidaTableSlot);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
