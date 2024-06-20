using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Service.Interface;
using EXE201_3CBilliard_Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.Post
{
    [Route("api/v1.0/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, IMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<CommentResponse>> CreateComment(CommentRequest commentRequest)
        {
            var commentResponse = await _commentService.CreateCommentAsync(commentRequest);
            return Ok(commentResponse);
        }

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetCommentsForPost(long postId)
        {
            var comments = await _commentService.GetCommentsForPostAsync(postId);
            return Ok(comments);
        }

        [HttpDelete("{cmtId}")]
        public async Task<ActionResult> DelCommentPost(long cmtId)
        {
            var result = await _commentService.DeleteCommentAsync(cmtId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
