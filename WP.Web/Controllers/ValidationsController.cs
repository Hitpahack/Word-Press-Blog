using Microsoft.AspNetCore.Mvc;
using WP.DTOs;
using WP.Services;
using WP.Web.Models;

namespace WP.Web.Controllers
{
    public class ValidationsController : Controller
    {
        private readonly IUserService _userService;

        public ValidationsController(IUserService userService)
        {
            _userService = userService;
        }
        [AcceptVerbs("GET", "POST")]
        [Route("CheckEmail")]
        public async Task<IActionResult> CheckEmail(string UserEmail, ulong userid = 0)
        {
            ApiResponse<bool> isExist = await _userService.CheckEmailExistAsync(UserEmail, userid);
            if (isExist.Data)
            {
                return Json(false); // Email already exists
            }
            return Json(true); // Email is available
        }

        [AcceptVerbs("GET", "POST")]
        [Route("CheckUsername")]
        public async Task<IActionResult> CheckUsername(string UserLogin, ulong userid = 0)
        {
            ApiResponse<bool> isExist = await _userService.CheckUsernameExistAsync(UserLogin, userid);
            if (isExist.Data)
            {
                return Json(false); // Email already exists
            }
            return Json(true); // Email is available
        }

       
    }
}
