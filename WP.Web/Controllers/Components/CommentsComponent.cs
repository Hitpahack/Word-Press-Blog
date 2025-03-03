using Microsoft.AspNetCore.Mvc;

namespace WP.Web.Controllers.Components
{
    public class CommentsComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _context;
        public CommentsComponent(IHttpContextAccessor context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
