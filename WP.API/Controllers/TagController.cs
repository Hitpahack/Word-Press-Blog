using Microsoft.AspNetCore.Mvc;
using WP.Core;
using WP.Data;
using WP.DTOs;
using WP.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WP.API.Controllers
{
    [Route("api/tag")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagController> _logger;
        private readonly ITokenService _tokenService;

        public TagController(ITagService tagService, ILogger<TagController> logger, ITokenService tokenService)
        {
            _tagService = tagService;
            _logger = logger;
            _tokenService = tokenService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateTag(TagRequestDto tag)
        {
            if (string.IsNullOrWhiteSpace(tag.Name))
            {
                return BadRequest(new { message = "Name of tag is required" });
            }
            var createTag= await _tagService.AddTagAsync(tag);
            return Ok(new { message = "Tag created successfully", tag });
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllTag()
        {
            var tags = await _tagService.GetAllTagAsync();
            return Ok(tags);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTag(List<ulong> Ids)
        {
            if (Ids.Count == 0 || Ids == null)
                return BadRequest("IDs cannot be empty.");
            await _tagService.DeleteTagAsync(Ids);
            return Ok("Tag deleted successfully.");
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateTag(TagDto tag)
        {
            if (tag == null || tag.Id == 0)
                return BadRequest(new { message = "Invalid data" });

            bool isUpdated = await _tagService.UpdateTagAsync(tag);

            if (!isUpdated)
                return NotFound(new { message = "Tag not found" });

            return Ok(new { message = "Tag updated successfully" });
        }

        [HttpPut("quick-update")]
        public async Task<IActionResult> QuickUpdateTag(WpTerm tag)
        {
            if (tag == null || tag.TermId == 0)
                return BadRequest(new { message = "Invalid data" });

            await _tagService.QuickUpdateTagAsync(tag);
            return Ok(new { message = "Tag updated successfully" });
        }
    }
}
