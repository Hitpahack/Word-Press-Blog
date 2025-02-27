using Microsoft.AspNetCore.Mvc;
using WP.Data.Repositories;
using WP.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WP.API.Controllers
{
    [Route("api/media")]
    [ApiController]
    public class MediaController : ControllerBase
    { 
        private readonly IMediaService _mediaService;
        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpPost("upload-media")]
        public async Task<IActionResult> UploadMedia(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file uploaded" });

            using var stream = file.OpenReadStream();
            await _mediaService.UploadMediaAsync(file);
            return Ok(new { message = "Media uploaded successfully!" });
        }
    }
}
