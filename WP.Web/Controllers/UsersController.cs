using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Security.Claims;
using WP.API.Controllers;
using WP.DTOs;
using WP.EDTOs.Users;
using WP.Services;
using WP.Web.Models;

namespace WP.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly Service.Users.IUsersService _userServic;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        public UsersController(IUserService userService, Service.Users.IUsersService userServic, ILogger<UserController> logger, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _userService = userService;
            _userServic = userServic;
            _logger = logger;
            _mapper = mapper;
            _httpContext = httpContext;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetAllUsersAsync();
            return View(result.Data);
        }
        [HttpPost]
        public async Task<IActionResult> GetUsersData([FromBody] UsersPagingRequest search)
        {
            var result = await _userServic.GetUsersPaged(search);
            return Json(result.Data);
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
        public async Task<IActionResult> EditUser(ulong UserId, EditUserDto model)
         {
            if (!ModelState.IsValid || UserId<=0)
                return View(model);

            var result = await _userService.UpdateUserAsync(UserId, model);
            if (!result.Success)
            {
                _logger.LogError(result.Message);
                return BadRequest(result);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string user)
        {
            var Id = user.Split(',').Select(ulong.Parse).ToList();
            var dtoList = new List<(EditUserDto EditUser, DeleteUserDto DeleteUser)>();
            foreach (var id in Id)
            {
                var result = await _userService.GetUserByIdAsync(id);
                if (result != null)
                {
                    var deleteUserDto = _mapper.Map<DeleteUserDto>(result);
                    dtoList.Add((result, deleteUserDto));
                }
            }
            var allRole = await _userService.GetAllAdminUserAsync();
            var adminUsers = allRole.Data.Where(r => r.Role == "administrator");
            ViewBag.AdminRoles= adminUsers;
            return View(dtoList);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUsers(string selectedIds, string Action, ulong? NewOwnerId = null )
        {
            if (string.IsNullOrWhiteSpace(selectedIds))
            {
                return BadRequest(new { Success = false, Message = "No user IDs provided." });
            }
            List<ulong> userIds;
            try
            {
                userIds = selectedIds.Split(',')
                                     .Select(id => ulong.Parse(id.Trim()))
                                     .ToList();
            }
            catch
            {
                return BadRequest(new { Success = false, Message = "Invalid user ID format." });
            }
            if (string.IsNullOrWhiteSpace(Action) || (Action != "delete" && Action != "assign"))
            {
                return BadRequest(new { Success = false, Message = "Invalid action type. Use 'delete' or 'assign'." });
            }

            ulong loggedUserId = Convert.ToUInt64(_httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userIds.Contains(loggedUserId))
            {
                return BadRequest( "Logged in used can not be deleted");
            }

            var response = await _userServic.DeleteUsers(selectedIds, Action, NewOwnerId);
            if (!response.Success)
            {
                _logger.LogError(response.Message);
                return BadRequest(response);
            }
            else
                return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> GetFilteredUsers(string filter)
        {
            return null;
        }

    }
}
