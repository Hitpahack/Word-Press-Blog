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
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest(new { message = "Email is required" });

            bool success = await _userService.SendPasswordResetEmailAsync(dto);
            if (!success)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = "Password reset link sent successfully" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.NewPassword) || string.IsNullOrWhiteSpace(dto.Token))
                return BadRequest(new { message = "All fields are required" });

            bool success = await _userService.ResetPasswordAsync(dto);
            if (!success)
                return BadRequest(new { message = "Invalid request" });

            return Ok(new { message = "Password reset successful" });
        }
    }
}
