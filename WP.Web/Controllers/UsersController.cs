using AutoMapper;
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
        private readonly IMapper _mapper;
        public UsersController(IUserService userService, ILogger<UserController> logger, IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetAllUsersAsync();
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> GetUsersData([FromBody] SearchModel search)
        {
            var result = await _userService.GetUsersPageAsync(search);
            return Json(result);
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

        public async Task<IActionResult> EditUser(ulong user)
        {
            var result = await _userService.GetUserByIdAsync(user);
            var udpateDto = _mapper.Map<EditUserDto>(result);
            ViewBag.Roles = StaticData.GetRolesSelected(udpateDto.Role);
            return View(udpateDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(ulong user, EditUserDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.UpdateUserAsync(user, model);
            if (!result.Success)
            {
                _logger.LogError(result.Message);
                return BadRequest(result);
            }

            return RedirectToAction("Index");
        }

    }
}
