using Microsoft.AspNetCore.Mvc;
using WP.EDTOs.Yoast;
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
            if(string.IsNullOrEmpty(content))
                return Json(new Dictionary<string, string>());

            var result = await _yoastServices.FocusKeyphrase(content);
            return Json(result);
        }
        [HttpPost]
        [Route("getreadabaility")]
        public async Task<IActionResult> GetReadabaility(string content)
        {
            if (string.IsNullOrEmpty(content))
                return Json(new List<EDTOs.Yoast.YOAST_DTO>());

            var result = await _yoastServices.Readability(content);
            return Json(result);
        }
        [HttpPost]
        [Route("getseoanylisis")]
        public async Task<IActionResult> GetSeoAnylisis(SEOAnalyzer reqDto)
        {
            var result = await _yoastServices.SeoAnyliss(reqDto);
            return Json(result);
        }
    }
}
