using System.ComponentModel.DataAnnotations;

namespace WP.DTOs;

public class UserDtos
{
    public class UserRegisterDTO
    {
        [Required]
        public string UserLogin { get; set; }

        [Required]
        public string UserPass { get; set; }

        public string UserNicename { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string UserEmail { get; set; }

        public string UserUrl { get; set; }

        public DateTime UserRegistered { get; set; }

        public string UserActivationKey { get; set; }

        public int UserStatus { get; set; }

        public string DisplayName { get; set; }
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

    public class ResetPasswordDto
    {

    }
}
