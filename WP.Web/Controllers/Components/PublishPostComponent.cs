using Microsoft.AspNetCore.Mvc;

namespace WP.Web.Controllers.Components
{
    public class PublishPostComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _context;
        public PublishPostComponent(IHttpContextAccessor context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
