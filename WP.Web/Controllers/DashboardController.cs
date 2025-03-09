using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WP.Service;

namespace WP.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {

        public DashboardController()
        {
            
        }
        public async Task<IActionResult> Index()
        {
           
            return View();
        }
    }
}
