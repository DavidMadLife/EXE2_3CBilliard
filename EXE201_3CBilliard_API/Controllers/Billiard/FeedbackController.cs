﻿using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.Billiard
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackResponse>>> GetAll()
        {
            var feedbacks = await _feedbackService.GetAllFeedbackAsync();
            return Ok(feedbacks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackResponse>> GetById(long id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            if (feedback == null)
                return NotFound();

            return Ok(feedback);
        }

        [HttpPost]
        public async Task<ActionResult<FeedbackResponse>> Create([FromBody] FeedbackRequset request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdFeedback = await _feedbackService.CreateFeedbackAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdFeedback.FeedbackId }, createdFeedback);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FeedbackResponse>> Update(long id, [FromBody] FeedbackRequset request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedFeedback = await _feedbackService.UpdateFeedbackAsync(id, request);
                return Ok(updatedFeedback);
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
                var result = await _feedbackService.DeleteFeedbackAsync(id);
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
