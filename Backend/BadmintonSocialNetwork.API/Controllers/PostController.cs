using BadmintonSocialNetwork.Service.DTOs;
using BadmintonSocialNetwork.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonSocialNetwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        [Route("upload-post")]
        public async Task<IActionResult> UploadPost([FromBody] PostCM postCM)
        {
            try
            {
                return null;
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
