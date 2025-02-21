using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WP.DTOs;

public class UserDtos
{
    public class UserRegisterDTO
    {
        [Required]
        [JsonPropertyName("userName")]
        public string UserLogin { get; set; }


        [Required]
        [JsonPropertyName("password")]
        public string UserPass { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [JsonPropertyName("email")]
        public string UserEmail { get; set; }
        
    }

        public class UserLoginDTO
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

    public class UserDto
    {
        public ulong Id { get; set; }
        public string UserLogin { get; set; }
        public string UserEmail { get; set; }
        public string UserNicename { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }
        public int Posts { get; set; }
    }

    public class ForgotPasswordDTO
    {
        public string Email { get; set; }
    }
    public class ResetPasswordDTO
    {
        public string Token { get; set; }  // Reset token from email
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }

}
