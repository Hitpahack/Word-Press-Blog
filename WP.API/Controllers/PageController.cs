using Microsoft.AspNetCore.Mvc;
using WP.Data;
using WP.DTOs;
using WP.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WP.API.Controllers
{
    [Route("api/page")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [HttpGet(("get-pages"))]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPages()
        {
            var pages = await _pageService.GetAllPageAsync();
            if (pages == null)
            {
                return NotFound("No posts found.");
            }
            return Ok(pages);
        }

        [HttpPost("create-page")]
        public async Task<IActionResult> CreatePage([FromBody] CreatePageDto page)
        {
            if (page == null)
            {
                return BadRequest("Page data should not be NULL");
            }
            var result = await _pageService.CreatePageAsync(page);
            return Ok("Page created successfully");
        }

        [HttpDelete("delete-page")]
        public async Task<IActionResult> DeletePage([FromBody] List<ulong> Ids)
        {
            if (Ids.Count == 0 || Ids == null)
                return BadRequest("Page IDs cannot be empty.");

            await _pageService.DeletePageAsync(Ids);
            return Ok("Pages deleted successfully");

        }

        [HttpPut("update-page")]

        public async Task<IActionResult> UpdatePage([FromRoute] ulong Id,[FromBody] UpdatePageDto page)
        {
            if (page == null || Id <= 0)
            {
                return BadRequest("Invalid post data.");
            }
            await _pageService.UpdatePageAsync(Id,page);
            return Ok("Page updated successfully.");
        }

        [HttpGet("search-page")]
        public async Task<ActionResult<WpPost>> GetPostByName(string pageName)
        {
            var page = await _pageService.GetPageByNameAsync(pageName);
            if (page == null)
                return null;
            return Ok(page);
        }
    }
}
