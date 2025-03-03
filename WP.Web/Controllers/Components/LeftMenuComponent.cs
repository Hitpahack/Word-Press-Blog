using Microsoft.AspNetCore.Mvc;

namespace WP.Web.Controllers.Components
{
    public class LeftMenuComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _context;
        public LeftMenuComponent(IHttpContextAccessor context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
