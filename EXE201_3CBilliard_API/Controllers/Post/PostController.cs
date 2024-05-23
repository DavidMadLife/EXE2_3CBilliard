using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Service.Interface;
using EXE201_3CBilliard_Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.Post
{
    [Route("api/v1.0/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<PostResponse>>> GetAll()
        {
            var posts = await _postService.GetAllPostsAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostResponse>> GetById(long id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost("create")]
        public async Task<ActionResult<PostResponse>> Create([FromBody] PostRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPost = await _postService.CreatePostAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdPost.Id }, createdPost);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PostResponse>> Update(long id, [FromBody] PostRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedPost = await _postService.UpdatePostAsync(id, request);
                return Ok(updatedPost);
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
                await _postService.DeletePostAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
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
                var response = await _postService.ActivatePostAsync(id, noteRequest);
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
                var response = await _postService.RejectPostAsync(id, noteRequest);
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
    }
}
