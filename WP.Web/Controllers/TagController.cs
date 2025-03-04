using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WP.Web.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        public TagController()
        {
            
        }
        public IActionResult Index()
        {
            
            return View();
        }

        
    }
}
