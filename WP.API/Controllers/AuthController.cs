using Microsoft.AspNetCore.Mvc;
using WP.Services;
using static WP.DTOs.UserDtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WP.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            if (dto ==null || string.IsNullOrWhiteSpace(dto.Email))
            {
                _logger.LogWarning("ForgotPassword request failed: Email is missing");
                return BadRequest(new { message = "Email is required" });
            }
            _logger.LogInformation("Processing forgot password request for email: {Email}", dto.Email); 
            bool success = await _userService.SendPasswordResetEmailAsync(dto);
            if (!success)
            {
                _logger.LogWarning("ForgotPassword failed: No user found with email {Email}", dto.Email);
                return NotFound(new { message = "User not found" });
            }
            _logger.LogInformation("Password reset link sent successfully to {Email}", dto.Email);
            return Ok(new { message = "Password reset link sent successfully" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("ResetPassword request failed: Request body is null");
                return BadRequest(new { message = "Invalid request data" });
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                _logger.LogWarning("ResetPassword request failed: Missing Email");
                return BadRequest(new { message = "Email is required" });
            }

            if (string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                _logger.LogWarning("ResetPassword request failed: Missing NewPassword");
                return BadRequest(new { message = "New password is required" });
            }
            if (string.IsNullOrWhiteSpace(dto.Token))
            {
                _logger.LogWarning("ResetPassword request failed: Missing Token");
                return BadRequest(new { message = "Reset token is required" });
            }

            _logger.LogInformation("Processing password reset for email: {Email}", dto.Email);
            bool success = await _userService.ResetPasswordAsync(dto);
            if (!success)
            {
                _logger.LogWarning("ResetPassword failed: Invalid request for email {Email}", dto.Email);
                return BadRequest(new { message = "Invalid request" });
            }
            _logger.LogInformation("Password reset successful for email: {Email}", dto.Email);
            return Ok(new { message = "Password reset successful" });
        }
    }
}
