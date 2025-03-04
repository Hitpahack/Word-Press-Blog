using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WP.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        public CategoryController()
        {
            
        }
        public IActionResult Index()
        {
            
            return View();
        }

        
    }
}
