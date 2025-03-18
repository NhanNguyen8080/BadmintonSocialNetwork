using BadmintonSocialNetwork.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonSocialNetwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;


        [HttpPost]
        [Route("like-unlike-post/{postId}")]
        public async Task<IActionResult> LikeUnlikePost(Guid postId)
        {
            try
            {
                var response = await _likeService.LikeUnlikePost(postId);
                if (response.IsSuccess)
                {
                    return Ok(response.Data);
                }
                return BadRequest(response.Message);
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
