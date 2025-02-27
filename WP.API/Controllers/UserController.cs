using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core.Tokenizer;
using WP.Core;
using WP.Data;
using WP.DTOs;
using WP.Services;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WP.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _usersService;
        private readonly ILogger<UserController> _logger;
        private readonly ITokenService _tokenService;
        public UserController(IUserService usersService, ILogger<UserController> logger, ITokenService tokenService)
        {
            _usersService = usersService;
            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.UserEmail) || string.IsNullOrWhiteSpace(dto.UserLogin) || string.IsNullOrWhiteSpace(dto.UserPass))
            {
                return BadRequest(new { message = "Username, Email, and Password are required" });
            }
            bool CheckUserExists = await _usersService.CheckUserExistsAsync(dto.UserLogin,dto.UserEmail);
            if (CheckUserExists)
            {
                _logger.LogWarning($"Registration attempt failed: Username '{dto.UserLogin}' or Email '{dto.UserEmail}' already exists.");
                return Conflict(new { message = "Username or Email already exists" });
            }
            
            var result = await _usersService.RegisterUserAsync(dto);

            if (result ==null)
            {
                _logger.LogError($"Failed to register user: {dto.UserLogin}");
                return StatusCode(500, new { message = "Failed to register user due to an internal error" });
            }
            _logger.LogInformation($"User '{dto.UserLogin}' registered successfully.");
            return Ok(new { message = "User registered successfully"});
        }

        [HttpPost("login")] 
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            string userIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            if(dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "Username and Password are required " });
            }
            var user = await _usersService.AuthenticateUserAsync(dto, userIp);
            if (user.Message == "Invalid username or password." || user.Message == "Too many failed attempts. Try again later.")
            {
                _logger.LogWarning($"Failed login attempt for user: {dto.Username} because {user.Message}");
                return Unauthorized(new { message = user.Message });
            }
            var token = _tokenService.GenerateToken(user);
            HttpContext.Session.SetString("Token", token);
            return Ok(new {token,user});
        }

        [HttpGet("get-users")]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _usersService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpDelete("delete-user")]
        public async Task<IActionResult> DeleteUsers([FromBody] List<ulong> Ids)
        {
            if (Ids.Count == 0 || Ids == null)
                return BadRequest("User IDs cannot be empty.");

            await _usersService.DeleteUserAsync(Ids);
            return Ok("Users deleted successfully.");
        }

        [HttpPut("update-user")]

        public async Task<IActionResult> UpdateUsers([FromBody] WpUser userDto)
        {
            if (userDto == null || userDto.Id == 0)
            {
                return BadRequest("Invalid user data.");
            }
            await _usersService.UpdateUserAsync(userDto);
            return Ok("User updated successfully.");
        }

        [HttpGet("generate-password")]
        public IActionResult GeneratePassword()
        {
            var password = PasswordHelper.GeneratePassword();
            var strength = PasswordHelper.GetPasswordStrength(password);

            return Ok(new { password, strength });
        }

        [HttpPost("check-strength")]
        public IActionResult CheckPasswordStrength([FromBody] PasswordRequest request)
        {
            var strength = PasswordHelper.GetPasswordStrength(request.Password);
            return Ok(new { strength });
        }


    }
}
