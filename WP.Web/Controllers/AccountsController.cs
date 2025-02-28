using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Claims;
using WP.Core;
using WP.Services;
using WP.Web.Models;
using static WP.DTOs.UserDtos;

namespace WP.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AccountsController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _userService.AuthenticateUserAsync(model, HttpContext.GetClientIP());
            if (!response.Success)
            {
                ModelState.AddModelError("", response.Message);
                return View(model);
            }
            var token = _tokenService.GenerateToken(response.Data, async (d) =>
            {
                var identity = new ClaimsIdentity(d.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            });

            if (HttpContext.User.Identity.IsAuthenticated)
            {

            }


            return RedirectToAction("Index", "Dashboard");
        }


        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(UserRegisterDTO model)
        {
            if (ModelState.IsValid)
                return View(model);


            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordDTO model)
        {
            if (ModelState.IsValid)
                return View(model);


            return View();
        }

    }
}
