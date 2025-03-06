using Microsoft.AspNetCore.Mvc;
using WP.Data;
using WP.DTOs;
using WP.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet(("get-posts"))]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetAllPosts(SearchModel search)
        {
            

            var posts = await _postService.GetAllPostsAsync(search);

           

            
            return Ok(posts);
        }
            
        [HttpPost("create-post")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto post)
        {
            if (post == null)
            {
                _logger.LogWarning("Post creation failed: Received NULL post data.");
                return BadRequest(new ApiResponse<string>(false, "Post data should not be NULL.", null, 400));
            }
            _logger.LogInformation("Creating a new post: {Title}", post.Title);
            var result = await _postService.CreatePostAsync(post);

            if (!result.Success)
            {
                _logger.LogWarning("Post creation failed: {Message}", result.Message);
                return StatusCode(500, result);
            }

            _logger.LogInformation("Post created successfully with ID: {PostId}", result.Data);
            return Ok(result);
        }

        [HttpDelete("delete-post")]
        public async Task<IActionResult> DeletePost([FromBody] List<ulong> Ids)
        {
            if (Ids == null || Ids.Count == 0)
            {
                _logger.LogWarning("Delete request failed: No post IDs provided.");
                return BadRequest(new { message = "Post IDs cannot be empty." });
            }

            _logger.LogInformation("Received request to delete {Count} posts.", Ids.Count);
            var response = await _postService.DeletePostAsync(Ids);
            if (!response.Success)
            {
                _logger.LogWarning("Delete request failed: {Message}", response.Message);
                return StatusCode(response.StatusCode, new { message = response.Message });
            }

            _logger.LogInformation("Successfully deleted {Count} posts.", response.Data);
            return Ok(new { message = response.Message });

        }

        [HttpPut("update-post")]

        public async Task<IActionResult> UpdatePost([FromRoute] ulong Id, [FromBody] UpdatePostDto post)
        {
            if (post == null)
            {
                _logger.LogWarning("Update request failed: No data provided.");
                return BadRequest(new { message = "Invalid request data." });
            }
            _logger.LogInformation("Received request to update post ID {PostId}.", Id);
            var response = await _postService.UpdatePostAsync(Id, post);
            if (!response.Success)
            {
                _logger.LogWarning("Update request failed: {Message}", response.Message);
                return StatusCode(response.StatusCode, new { message = response.Message });
            }

            _logger.LogInformation("Successfully updated post ID {PostId}.", Id);
            return Ok(new { message = response.Message });
        }

        [HttpGet("search-post")]

        public async Task<ActionResult<WpPost>> GetPostByName(string postName)
        {
            var post = await _postService.GetPostByNameAsync(postName);
            if (post == null)
                return null;
            return Ok(post);
        }
    }
}
