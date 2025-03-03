using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WP.Web.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        public PostsController()
        {
            
        }
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult AddPost()
        {

            return View();
        }

        public IActionResult AddPost()
        {

            return View();
        }
    }
}
