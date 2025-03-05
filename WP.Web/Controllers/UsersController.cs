using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using WP.API.Controllers;
using WP.DTOs;
using WP.Services;
using WP.Web.Models;

namespace WP.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UsersController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetAllUsersAsync();
            return View(result.Data);
        }
        public async Task<IActionResult> Index(SearchModel search)
        {
            var result = await _userService.GetAllUsersAsync();
            return View(result.Data);
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

            var result = await _userService.CreateUserAsync(model);
            if (!result.Success)
            {
                _logger.LogError(result.Message);
                return BadRequest(result);
            }

            return RedirectToAction("Index");
        }

        public IActionResult EditUser()
        {
            ViewBag.Roles = StaticData.GetRoles;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(ulong id, UpdateUserDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.UpdateUserAsync(id,model);
            if (!result.Success)
            {
                _logger.LogError(result.Message);
                return BadRequest(result);
            }

            return RedirectToAction("Index");
        }

    }
}
