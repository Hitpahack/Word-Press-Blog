using Microsoft.AspNetCore.Mvc;
using WP.Core;
using WP.Data;
using WP.DTOs;
using WP.Services;
using static WP.DTOs.UserDtos;

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
            bool result = await _usersService.RegisterUserAsync(dto);
            if (!result)
                return BadRequest("Username already exists");

            return Ok("User registered successfully");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            if(dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "Username and Password are required " });
            }
            var user = await _usersService.AuthenticateUserAsync(dto);
            if (user == null)
            {
                _logger.LogWarning($"Failed login attempt for user: {dto.Username}");
                return Unauthorized(new { messsage = "Invalid credentials" });
            }
            var token = _tokenService.GenerateToken(user);
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
