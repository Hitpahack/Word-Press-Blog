using Abp.Runtime.Security;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WP.DTOs;
using WP.EDTOs.Post;
using WP.Service.Categories;
using WP.Service.Yoast;
using WP.Services;

namespace WP.Web.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly Service.IPostService _postServic;
        private readonly Service.Medias.IMediaService _mediaService;
        private readonly ILogger<PostsController> _logger;
        private readonly IMapper _mapper;
        private readonly IYoastServices _yoastServices;
        private readonly ITermsService _termsService;
        private static List<string> AllTags = new List<string>();
        public PostsController(IPostService postService, ITermsService termsService, Service.Medias.IMediaService mediaService, Service.IPostService postServic, ILogger<PostsController> logger, IMapper mapper, IYoastServices yoastServices)
        {
            _postServic = postServic;
            _postService = postService;
            _logger = logger;
            _termsService = termsService;
            _mapper = mapper;
            _yoastServices = yoastServices;
            _mediaService = mediaService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = (await _termsService.GetAllCategories()).Data.Select(s=> new SelectListItem { Text = s.Name, Value = s.Term_Id.ToString()}).ToList();
			categories.Insert(0, new SelectListItem { Value = "0", Text = "All Categories" });
			ViewBag.Categories = categories;
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
            EDTOs.POST_DTO model = new EDTOs.POST_DTO(); 
            if (post > 0)
            {
                var postData = await _postServic.GetPost(post);
                model = postData.Data;
                model.Seo = (await _yoastServices.GetPostSEO(post));
            }
            
            model.CategoriesItems = (await _termsService.GetCategories(0, post)).Data;
            model.TagsItem = (await _termsService.GetTags(0, post)).Data;
            ViewBag.PageTypes = _postServic.GetPageTypes;
            ViewBag.ArticleTypes = _postServic.GetArticleTypes;
            //AllTags = (await _termsService.GetTags(0)).Data.Select(s => s.Name).ToList();
			return View(model);
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
            
            if (!reuslt.Success)
            {
                model.CategoriesItems = (await _termsService.GetCategories(0, post)).Data;
                model.TagsItem = (await _termsService.GetTags(0, post)).Data;
                ViewBag.PageTypes = _postServic.GetPageTypes;
                ViewBag.ArticleTypes = _postServic.GetArticleTypes;
                _logger.LogError(reuslt.Message);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(ulong catid, string cat)
        {

            if (string.IsNullOrEmpty(cat) || catid <= 0)
                return Json("required field missing");

            var isSuccess = await _termsService.AddCateroty(cat, catid);
            return Json(isSuccess);
        }


        [HttpGet]
        public JsonResult GetTags(string term, List<string> selectedTags = null)
        {
            try
            {
                selectedTags ??= new List<string>(); // Ensure selectedTags is not null

            var availableTags = AllTags
            .Where(tag => tag.ToLower().Contains(term.ToLower()) && !selectedTags.Select(t => t.ToLower()).Contains(tag.ToLower()))
            .Select(tag => new { value = tag }) // Format for jQuery UI Autocomplete
            .ToList();

                return Json(availableTags);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> RemoveFeaturedImage(ulong postid)
        {
            var response = await _mediaService.Remove_FeaturedImage(postid);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(ulong id)
        {
            var post = await _postServic.DeletePost(id);
            return Json(post);
        }

        [HttpPost]  
        public async Task<IActionResult> DeletePosts(ulong[] selectedIds)
        {
            var post = await _postServic.DeletePost(selectedIds);
            return Json(post);
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredPosts(string filter)
        {
            return null;
        }
    }
}
 