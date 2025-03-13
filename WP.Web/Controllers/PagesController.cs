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
    public class PagesController : Controller
    {
        private readonly IPageService _pageService;
        private readonly Service.IPostService _postService;
        private readonly ILogger<PagesController> _logger;
        private readonly IMapper _mapper;
        private readonly ITermsService _termsService;

        public PagesController(IPageService pageService, Service.IPostService postService, ILogger<PagesController> logger, IMapper mapper, ITermsService termsService)
        {
            _pageService = pageService;
            _postService = postService;
            _logger = logger;
            _mapper = mapper;
            _termsService = termsService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetPagesData([FromBody] PostPagingRequest search)
        {
            var result = await _postService.GetPagesPaged(search);
            return Json(result.Data);
        }

        public async Task<IActionResult> AddPage(ulong page = 0)
        {
            ViewBag.Id = page;
            if (page > 0)
            {
                var postData = await _postService.GetPage(page);
                return View(postData.Data);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPage(EDTOs.WP_PAGE_ADD_DTO model, ulong page = 0)
        {

            if (!ModelState.IsValid)
                return View(model);

            ViewBag.Id = page;
            var udi = HttpContext.User.Identity.GetUserId();
            model.Post_Author = (ulong)udi;
            ApiResponse<ulong> result;
            var reuslt = await _postService.AddUpdatePage(model, page);
            if (!reuslt.Success)
            {
                model.CategoriesItems = (await _termsService.GetCategories(0, page)).Data;
                model.TagsItem = (await _termsService.GetTags(page)).Data;
                _logger.LogError(reuslt.Message);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePage(ulong id)
        {
            var post = await _postService.DeletePost(id);
            if (post == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePages(ulong[] selectedIds)
        {
            var post = await _postService.DeletePost(selectedIds);
            if (post.Success)
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index") });
            }
            return Json(new { success = false, message = "Failed to delete pages." });
        }
    }
}
