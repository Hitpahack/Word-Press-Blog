using Microsoft.AspNetCore.Mvc;
using WP.Services;

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
        public IActionResult CheckEmail(string email)
        {
            if (ExistingEmails.Contains(email.ToLower()))
            {
                return Json(false); // Email already exists
            }
            return Json(true); // Email is available
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckUsername(string username)
        {
            if (ExistingUsernames.Contains(username.ToLower()))
            {
                return Json(false); // Username already taken
            }
            return Json(true); // Username is available
        }
    }
}
