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

        public PostController(IPostService postService)
        {
            _postService = postService; 
        }

        [HttpGet(("get-posts"))]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
        {
            var posts =  await _postService.GetAllPostsAsync();
            if (posts == null || !posts.Any())
            {
                return NotFound("No posts found.");
            }
            return Ok(posts);
        }

        [HttpPost("create-post")]
        public async Task<IActionResult> CreatePost([FromBody] WpPost post)
        {
            if (post == null)
            {
                return BadRequest("Post data should not be NULL");
            }
            var result = await _postService.CreatePostAsync(post);
            return Ok("Post created successfully");
        }

        [HttpDelete("delete-post")]
        public async Task<IActionResult> DeletePost([FromBody] List<ulong> Ids)
        {
            if(Ids.Count == 0 || Ids == null)
                return BadRequest("Post IDs cannot be empty.");

            await _postService.DeletePostAsync(Ids);
            return Ok("Posts deleted successfully");

        }

        [HttpPut("update-post")]

        public async Task<IActionResult> UpdatePost([FromBody] WpPost post)
        {
            if (post == null || post.Id <= 0)
            {
                return BadRequest("Invalid post data.");
            }
            await _postService.UpdatePostAsync(post);
            return Ok("User updated successfully.");
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
