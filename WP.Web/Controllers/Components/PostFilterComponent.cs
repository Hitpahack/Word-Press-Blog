using Microsoft.AspNetCore.Mvc;
using WP.Service;



namespace WP.Web.Controllers.Components
{
    public class PostFilterComponent : ViewComponent
    {
        private readonly IPostService _postService;
        public PostFilterComponent(IPostService postService)
        {
            _postService = postService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filters = await _postService.GetFiltersAsync();
            return View(filters);
        }
    }
}
