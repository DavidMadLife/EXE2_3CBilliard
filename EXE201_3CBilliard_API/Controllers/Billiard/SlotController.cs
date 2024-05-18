using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.Billiard
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SlotResponse>>> GetAll()
        {
            var slots = await _slotService.GetAllSlotsAsync();
            return Ok(slots);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SlotResponse>> GetById(long id)
        {
            var slot = await _slotService.GetSlotByIdAsync(id);
            if (slot == null)
                return NotFound();

            return Ok(slot);
        }

        [HttpPost]
        public async Task<ActionResult<SlotResponse>> Create([FromBody] SlotRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdSlot = await _slotService.CreateSlotAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdSlot.Id }, createdSlot);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SlotResponse>> Update(long id, [FromBody] SlotRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedSlot = await _slotService.UpdateSlotAsync(id, request);
                return Ok(updatedSlot);
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
                await _slotService.DeleteSlotAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}