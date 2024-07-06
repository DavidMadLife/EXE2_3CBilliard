using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using EXE201_3CBilliard_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.Notificate
{
    [Route("api/v1.0/notificates")]
    [ApiController]
    public class NotificateController : ControllerBase
    {
        private readonly INotificateService _notificateService;

        public NotificateController(INotificateService notificateService)
        {
            _notificateService = notificateService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<NotificateResponse>>> GetAll()
        {
            var notificates = await _notificateService.GetAllNotificateAsync();
            return Ok(notificates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificateResponse>> GetById(long id)
        {
            var notificate = await _notificateService.GetNotificateByIdAsync(id);
            if (notificate == null)
                return NotFound();

            return Ok(notificate);
        }

       /* [HttpPost("create")]
        public async Task<ActionResult<NotificateResponse>> Create([FromBody] NotificateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdNotificate = await _notificateService.CreateNotificateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdNotificate.Id }, createdNotificate);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NotificateResponse>> Update(long id, [FromBody] NotificateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedNotificate = await _notificateService.UpdateNotificateAsync(id, request);
                return Ok(updatedNotificate);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }*/

        [HttpPut("delete/{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await _notificateService.DeleteNotificateAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchNotificates(
             [FromQuery] string? title,
             [FromQuery] string? description,
             [FromQuery] NotificateStatus? status,
             [FromQuery] long? userId,
             [FromQuery] NotificationType? type,
             [FromQuery] string? billOrderCode,
             [FromQuery] string? billStatus,
             [FromQuery] int pageIndex = 1,
             [FromQuery] int pageSize = 10)
        {
            var (notificates, totalCount) = await _notificateService.SearchNotificatesAsync(
                title,
                description,
                status,
                userId,
                type,
                billOrderCode,
                billStatus,
                pageIndex,
                pageSize
            );
            return Ok(new { notificates, totalCount });
        }

    }
}
