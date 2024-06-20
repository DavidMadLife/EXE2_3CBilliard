using AutoMapper;
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
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<PostResponse>> CreatePost(PostRequest postRequest)
        {
            var postResponse = await _postService.CreatePostAsync(postRequest);
            return Ok(postResponse);
        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<PostResponse>> GetPostById(long postId)
        {
            var postResponse = await _postService.GetPostByIdAsync(postId);
            if (postResponse == null)
            {
                return NotFound();
            }
            return Ok(postResponse);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostResponse>>> GetPosts()
        {
            var posts = await _postService.GetPostsAsync();
            return Ok(posts);
        }

        [HttpPut("{postId}")]
        public async Task<ActionResult<PostResponse>> UpdatePost(long postId, PostRequest postRequest)
        {
            var updatedPost = await _postService.UpdatePostAsync(postId, postRequest);
            if (updatedPost == null)
            {
                return NotFound();
            }
            return Ok(updatedPost);
        }

        [HttpDelete("{postId}")]
        public async Task<ActionResult> DeletePost(long postId)
        {
            var result = await _postService.DeletePostAsync(postId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
