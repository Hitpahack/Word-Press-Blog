using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core.Tokenizer;
using WP.Core;
using WP.Data;
using WP.DTOs;
using WP.Services;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WP.API.Controllers
{

    // WordPress User Levels (Deprecated but Still Used by Some Plugins)
    // ------------------------------------------------------------
    // Role          | User Level | Capabilities
    // -------------|-----------|-------------------------------------------
    // Administrator | 10        | Full access (manage site, users, settings).
    // Editor       | 7         | Edit, publish, and delete any post.
    // Author       | 2         | Write and publish their own posts.
    // Contributor  | 1         | Write posts, but need approval to publish.
    // Subscriber   | 0         | Read content only (default for new users).



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
                return BadRequest(new ApiResponse<object>(false, "Username, Email, and Password are required.", null, 400));
            }
            bool CheckUserExists = await _usersService.CheckUserExistsAsync(dto.UserLogin, dto.UserEmail);
            if (CheckUserExists)
            {
                _logger.LogWarning($"Registration attempt failed: Username '{dto.UserLogin}' or Email '{dto.UserEmail}' already exists.");
                return Conflict(new ApiResponse<object>(false, "Username or Email already exists.", null, 409));
            }

            var result = await _usersService.RegisterUserAsync(dto);

            if (result.Data == null)
            {
                _logger.LogError($"Failed to register user: {dto.UserLogin}");
                return StatusCode(result.StatusCode, result);
            }
            _logger.LogInformation($"User '{dto.UserLogin}' registered successfully.");
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            string userIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            if (dto == null || string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new ApiResponse<object>(false, "Username and Password are required.", null, 400));
            }
            var user = await _usersService.AuthenticateUserAsync(dto, userIp);
            if (user.Message == "Invalid username or password." || user.Message == "Too many failed attempts. Try again later.")
            {
                _logger.LogWarning($"Failed login attempt for user: {dto.Username} because {user.Message}");
                return Unauthorized(user);
            }
            //var token = _tokenService.GenerateToken(user);
            //HttpContext.Session.SetString("Token", token);
            //return Ok(new {token,user});
            return StatusCode(user.StatusCode, user);
        }

        [HttpGet("get-users")]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            _logger.LogInformation("Fetching all users...");
            var response = await _usersService.GetAllUsersAsync();
            if (!response.Success || response.Data == null || !response.Data.Any())
            {
                _logger.LogWarning("No users found in the database.");
                return NotFound(new ApiResponse<List<UserDto>>(false, "No users found.", null, 404));
            }
            _logger.LogInformation($"Retrieved {response.Data.Count} users successfully.");
            return Ok(response);
        }

        [HttpDelete("delete-user")]
        public async Task<IActionResult> DeleteUsers([FromBody] List<ulong> Ids)
        {
            if (Ids.Count == 0 || Ids == null)
            {
                _logger.LogWarning("Delete request failed: User IDs list is empty or null.");
                return BadRequest(new ApiResponse<string>(false, "User IDs cannot be empty.", null, 400));
            }
            _logger.LogInformation($"Received request to delete {Ids.Count} users: {string.Join(", ", Ids)}");
            var response = await _usersService.DeleteUserAsync(Ids);
            if (!response.Success)
            {
                _logger.LogWarning("Delete request failed: Some or all users were not found.");
                return NotFound(response); // If users were not found/deleted, return 404
            }
            _logger.LogInformation($"Successfully deleted {Ids.Count} users.");
            return Ok(response);
        }

        [HttpPut("update-user")]

        public async Task<IActionResult> UpdateUsers(ulong Id,[FromBody] UpdateUserDto userDto)
        {
            if (userDto == null || Id == 0)
            {
                _logger.LogWarning("Update request failed: Invalid user data. User ID: {UserId}", Id);
                return BadRequest(new ApiResponse<string>(false, "Invalid user data.", null, 400));
            }
            _logger.LogInformation("Received request to update user with ID: {UserId}", Id);
            var response = await _usersService.UpdateUserAsync(Id,userDto);
            if (!response.Success)
            {
                _logger.LogWarning("Update request failed: User ID {UserId} not found.", Id);
                return NotFound(response);
            }

            _logger.LogInformation("User with ID {UserId} updated successfully.", Id);
            return Ok(response);
        }

        [HttpGet("generate-password")]
        public IActionResult GeneratePassword()
        {
            _logger.LogInformation("Generating a new password...");
            var password = PasswordHelper.GeneratePassword();
            var strength = PasswordHelper.GetPasswordStrength(password);

            _logger.LogInformation("Generated password successfully. Strength: {Strength}", strength);
            var response = new ApiResponse<object>(true, "Password generated successfully.", new { password, strength }, 200);
            return Ok(response);
        }

        [HttpPost("check-password-strength")]
        public IActionResult CheckPasswordStrength([FromBody] PasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Password))
            {
                _logger.LogWarning("Password strength check failed: Password is empty or null.");
                return BadRequest(new ApiResponse<string>(false, "Password cannot be empty.", null, 400));
            }
            _logger.LogInformation("Checking password strength...");
            var strength = PasswordHelper.GetPasswordStrength(request.Password);
            _logger.LogInformation("Password strength evaluated successfully. Strength: {Strength}", strength);
            var response = new ApiResponse<object>(true, "Password strength evaluated successfully.", new { strength }, 200);
            return Ok(response);
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.UserEmail) || string.IsNullOrWhiteSpace(user.UserLogin) || string.IsNullOrWhiteSpace(user.UserPass))
            {
                return BadRequest(new ApiResponse<object>(false, "Username, Email, and Password are required.", null, 400));
            }
            bool CheckUserExists = await _usersService.CheckUserExistsAsync(user.UserLogin, user.UserEmail);
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
        }


    }
}
