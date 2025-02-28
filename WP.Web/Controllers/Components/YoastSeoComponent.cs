using Microsoft.AspNetCore.Mvc;

namespace WP.Web.Controllers.Components
{
    public class YoastSeoComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
