using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WP.Web.Controllers
{
    [Authorize]
    public class MediaController : Controller
    {
        public MediaController()
        {
            
        }
        public IActionResult Index()
        {
            
            return View();
        }

        
    }
}
