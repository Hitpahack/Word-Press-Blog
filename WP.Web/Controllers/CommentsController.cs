using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WP.Web.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        public CommentsController()
        {
            
        }
        public IActionResult Index()
        {
            
            return View();
        }

        
    }
}
