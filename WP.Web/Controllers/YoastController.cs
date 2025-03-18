using Microsoft.AspNetCore.Mvc;
using WP.Service.Yoast;

namespace WP.Web.Controllers
{
    public class YoastController : Controller
    {
        private readonly IYoastServices _yoastServices;

        public YoastController(IYoastServices yoastServices)
        {
            _yoastServices = yoastServices;
        }
        [HttpPost]
        [Route("getfocuskeyphrase")]
        public async Task<IActionResult> GetFocusKeyphrase(string content)
        {
            var result = await _yoastServices.FocusKeyphrase(content);
            return Json(result);
        }
    }
}
