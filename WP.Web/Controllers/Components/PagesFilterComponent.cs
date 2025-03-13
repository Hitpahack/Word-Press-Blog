using Microsoft.AspNetCore.Mvc;
using WP.Service;

namespace WP.Web.Controllers.Components
{
    public class PagesFilterComponent : ViewComponent
    {
        private readonly IPostService _postService;
        public PagesFilterComponent(IPostService postService)
        {
            _postService = postService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filters = await _postService.GetPageFiltersAsync();
            return View(filters);
        }
    }
}
