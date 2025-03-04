using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WP.Web.Controllers
{
    [Authorize]
    public class PageController : Controller
    {
        public PageController()
        {
            
        }
        public IActionResult Index()
        {
            
            return View();
        }

        
    }
}
