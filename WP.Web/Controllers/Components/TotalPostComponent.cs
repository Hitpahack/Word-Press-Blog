using Microsoft.AspNetCore.Mvc;

namespace WP.Web.Controllers.Components
{
    public class TotalPostComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _context;
        public TotalPostComponent(IHttpContextAccessor context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
