using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201_3CBilliard_API.Controllers.Post
{
    [Route("api/v1.0/likes")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;
        private readonly IMapper _mapper;

        public LikeController(ILikeService likeService, IMapper mapper)
        {
            _likeService = likeService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<LikeResponse>> LikePost(LikeRequest likeRequest)
        {
            var likeResponse = await _likeService.LikePostAsync(likeRequest);
            return Ok(likeResponse);
        }

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<LikeResponse>>> GetLikesForPost(long postId)
        {
            var likes = await _likeService.GetLikesForPostAsync(postId);
            return Ok(likes);
        }

        [HttpDelete("{likeId}")]
        public async Task<ActionResult> UnlikePost(long likeId)
        {
            var result = await _likeService.UnlikePostAsync(likeId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
