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
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly Service.IPostService _postService;
        private readonly ILogger<PageController> _logger;
        private readonly IMapper _mapper;
        private readonly ITermsService _termsService;

        public PageController(IPageService pageService, Service.IPostService postService, ILogger<PageController> logger, IMapper mapper, ITermsService termsService)
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
        public async Task<IActionResult> AddPage(EDTOs.WP_PAGE_ADD_DTO model, ulong post = 0)
        {

            if (!ModelState.IsValid)
                return View(model);

            ViewBag.Id = post;
            var udi = HttpContext.User.Identity.GetUserId();
            model.Post_Author = (ulong)udi;
            ApiResponse<ulong> result;
            var reuslt = await _postService.AddUpdatePage(model, post);
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
    }
}
