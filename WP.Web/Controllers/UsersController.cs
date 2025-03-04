using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WP.DTOs;
using WP.Services;
using WP.Web.Models;

namespace WP.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        public IActionResult AddUser()
        {
            ViewBag.Roles = StaticData.GetRoles;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(CreateUserDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            
            bool CheckUserExists = await _userService.CheckUserExistsAsync(model.UserLogin, model.UserEmail);
            if (CheckUserExists)
            {
                _logger.LogWarning($"Registration attempt failed: Username '{user.UserLogin}' or Email '{user.UserEmail}' already exists.");
                return Conflict(new ApiResponse<object>(false, "Username or Email already exists.", null, 409));
            }
            var result = await _usersService.CreateUserAsync(user);

            if (result == null)
            {
                _logger.LogError($"Failed to register user: {user.UserLogin}");
                return StatusCode(500, new ApiResponse<object>(false, "Failed to register user due to an internal error.", null, 500));
            }
            _logger.LogInformation($"User '{user.UserLogin}' registered successfully.");
            return Ok(new ApiResponse<object>(true, "User registered successfully.", result, 201));
            if (!response.Success)
            {
                ModelState.AddModelError("", response.Message);
                return View(model);
            }

            return View();
        }
    }
}
