using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using EXE201_3CBilliard_Service.Interface;
using EXE201_3CBilliard_Service.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EXE201_3CBilliard_API.Controllers.Billiard
{
    [Route("api/v1.0/bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<BookingResponse>>> GetAll()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        /*[HttpGet("{id}")]
        public async Task<ActionResult<BookingResponse>> GetById(long id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }*/

        /*[HttpPost("create")]
        public async Task<ActionResult<BookingResponse>> Create([FromBody] BookingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdBooking = await _bookingService.CreateBookingAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdBooking.Id }, createdBooking);
        }*/

        [HttpPut("{id}")]
        public async Task<ActionResult<BookingResponse>> Update(long id, [FromBody] BookingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedBooking = await _bookingService.UpdateBookingAsync(id, request);
                return Ok(updatedBooking);
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
                var result = await _bookingService.DeleteBookingAsync(id);
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

        [HttpPost("club-owner-book-slot")]
        public async Task<IActionResult> ClubOwnerBookSlots(long userId, [FromBody] List<long> slotIds,[FromQuery] DateTime bookingDate)
        {
            try
            {
                var result = await _bookingService.ClubOwnerBookSlotsAsync(userId, slotIds, bookingDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("booking")]
        public async Task<IActionResult> BookMultipleSlots(long userId, [FromBody] List<long> slotIds, [FromQuery] DateTime bookingDate)
        {
            try
            {
                var result = await _bookingService.BookMultipleSlotsAsync(userId, slotIds, bookingDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("book-and-generate-bill")]
        public async Task<IActionResult> BookSlotsAndGenerateBill([FromForm] BookAndBillRequest request)
        {

            try
            {
                var billResponse = await _bookingService.BookSlotsAndGenerateBillAsync(request.UserId, request.BT_SlotIds, request.BookingDate, request.BillRequest, request.Image);
                return Ok(billResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("book-and-generate-bill-v2")]
        public async Task<IActionResult> BookSlotsAndGenerateBill([FromBody] CombinedBookAndBillRequest request)
        {
            try
            {
                var billResponse = await _bookingService.BookSlotsAndGenerateBillAsyncv2(request);
                return Ok(billResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }




        [HttpGet("search")]
        public async Task<IActionResult> SearchBookings([FromQuery] long? userId, [FromQuery] DateTime? createAt, [FromQuery] DateTime? bookingdate, [FromQuery] string? orderCode, [FromQuery] string? status, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _bookingService.SearchBookingsAsync(userId, createAt, bookingdate, orderCode, status, pageIndex, pageSize);
            Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());
            return Ok(result.bookings);
        }
    }
}
