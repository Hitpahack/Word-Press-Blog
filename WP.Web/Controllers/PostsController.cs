using jQueryDatatable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WP.API.Controllers;
using WP.DTOs;
using WP.Services;

namespace WP.Web.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostsController> _logger;
        public PostsController(IPostService postService, ILogger<PostsController> logger)
        {
            _postService = postService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetPostsData([FromBody] SearchModel search)
        {
            
            var result = await _postService.GetAllPostsAsync(search);
            return View();
        }
        

    }
}
