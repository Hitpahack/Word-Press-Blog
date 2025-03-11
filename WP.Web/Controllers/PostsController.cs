using Abp.Runtime.Security;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WP.DTOs;
using WP.EDTOs.Post;
using WP.Service.Categories;
using WP.Services;

namespace WP.Web.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly Service.IPostService _postServic;
        private readonly ILogger<PostsController> _logger;
        private readonly IMapper _mapper;
        private readonly ITermsService _termsService;
        public PostsController(IPostService postService, ITermsService termsService, Service.IPostService postServic, ILogger<PostsController> logger, IMapper mapper)
        {
            _postServic = postServic;
            _postService = postService;
            _logger = logger;
            _termsService = termsService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetPostsData([FromBody] PostPagingRequest search)
        {

            var result = await _postServic.GetPostPaged(search);
            return Json(result.Data);
        }
        public async Task<IActionResult> AddPost(ulong post = 0)
        {
            ViewBag.Id = post;
            if (post > 0)
            {
                var postData = await _postServic.GetPost(post);
                postData.Data.CategoriesItems = (await _termsService.GetCategories(0, post)).Data;
                postData.Data.TagsItem = (await _termsService.GetTags(post)).Data;
                return View(postData.Data);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(EDTOs.WP_POST_ADD_DTO model, ulong post = 0)
        {
            if (!ModelState.IsValid)
                return View(model);

            ViewBag.Id = post;
            var udi = HttpContext.User.Identity.GetUserId();
            model.Post_Author = (ulong)udi;
            ApiResponse<ulong> result;
            var reuslt = await _postServic.AddUpdatePost(model, post);
            //if (post > 0)
            //     result = await _postService.UpdatePostAsync(post, model);
            //else
            //     result = await _postService.CreatePostAsync(model);
            
            if (!reuslt.Success)
            {
                model.CategoriesItems = (await _termsService.GetCategories(0, post)).Data;
                model.TagsItem = (await _termsService.GetTags(post)).Data;
                _logger.LogError(reuslt.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(ulong id)
        {
            var post = await _postServic.DeletePost(id);
            if (post == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]  
        public async Task<IActionResult> DeletePosts(ulong[] selectedIds)
        {
            var post = await _postServic.DeletePost(selectedIds);
            if (post.Success)
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index") });
            }
            return Json(new { success = false, message = "Failed to delete posts." });
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredPosts(string filter)
        {
            return null;
        }
    }
}
 