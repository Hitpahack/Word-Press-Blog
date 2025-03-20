using Microsoft.AspNetCore.Mvc;
using WP.Service;

namespace WP.Web.Controllers.Components
{
    public class CommentsFilterComponent : ViewComponent
    {
        private readonly ICommentsService _commentsService;
        public CommentsFilterComponent(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filters = await _commentsService.GetFiltersAsync();
            return View(filters.Data);
        }
    }
}
