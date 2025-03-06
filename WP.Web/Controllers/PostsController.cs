using Abp.Runtime.Security;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public PostsController(IPostService postService, ILogger<PostsController> logger, IMapper mapper)
        {
            _postService = postService;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetPostsData([FromBody] SearchModel search)
        {

            var result = await _postService.GetAllPostsAsync(search);
            return Json(result.Data);
        }
        public async Task<IActionResult> AddPost(ulong post = 0)
        {
            if (post > 0)
            {
                var postData = await _postService.GetPost(post);
                var model = _mapper.Map<CreatePostDto>(postData);
                return View(model);
            }
            return View(new CreatePostDto());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(CreatePostDto model, ulong post = 0)
        {
            if (!ModelState.IsValid)
                return View(model);

            var udi = HttpContext.User.Identity.GetUserId();
            model.AuthorId = (ulong)udi;
            ApiResponse<ulong> result;
            if(post > 0)
                 result = await _postService.UpdatePostAsync(post, model);
            else
                 result = await _postService.CreatePostAsync(model);
            
            if (!result.Success)
            {
                _logger.LogError(result.Message);
                return BadRequest(result);
            }

            return RedirectToAction("Index");
        }
    }
}
