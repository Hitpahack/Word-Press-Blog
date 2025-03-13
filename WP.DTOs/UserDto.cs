using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace WP.DTOs;


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
    public bool Remember { get; set; }
}

public class UserResponseDTO
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Message { get; set; }
}

public class UserDto
{
    public ulong Id { get; set; }
    public string UserLogin { get; set; }
    public string UserEmail { get; set; }
    public string UserNicename { get; set; }
    public string DisplayName { get; set; }
    public string Role { get; set; }
    public long TotalPosts { get; set; }
    public string Avatar { get; set; } = "https://secure.gravatar.com/avatar/5f40d96b53f99022e02458fd88d3b8a2?s=96&d=mm&r=g";

    public string UserUrl { get; set; } = null!;
    public DateTime UserRegistered { get; set; }

    public string UserActivationKey { get; set; } = null!;

    public int UserStatus { get; set; }


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

public class CreateUserDto
{
    [DisplayName("Username")]
    [Remote("CheckUsername", "Validations", ErrorMessage = "Username already exist")]
    [Required]
    public string UserLogin { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Remote("CheckEmail", "Validations", ErrorMessage = "UserEmail already exist")]
    [Required]
    [EmailAddress]
    public string UserEmail { get; set; }
    public string? UserNicename { get; set; }
    public string? UserUrl { get; set; } = null!;
    [Required]
    public string Role { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one letter, one number, and one special character.")]
    public string UserPass { get; set; }
    public bool SendUserNotification { get; set; }

}

public class RegisterUserResponseDto
{
    public ulong Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}


public class UpdateUserDto
{
    public string Email { get; set; }
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Nickname { get; set; }
    public string UserUrl { get; set; }
    public string DisplayName { get; set; }
}
public class RoleDto
{
    public RoleDto(ulong userid, string role)
    {
        UserId = userid;
        Role = role;
    }
    public ulong UserId { get; set; }
    public string Role { get; set; }
}

public class EditUserDto
{
    public ulong UserId { get; set; }
    [DisplayName("Username")]
    //[Remote("CheckUsername", "Validations", AdditionalFields = "UserId", ErrorMessage = "Username already exist")]
    [Required]
    public string UserLogin { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
    //[Remote("CheckEmail", "Validations", AdditionalFields = "UserId", ErrorMessage = "UserEmail already exist")]
    [Required]
    [EmailAddress]
    public string UserEmail { get; set; }
    [DisplayName("Nickname")]
    public string? Nickname { get; set; }
    public string? UserUrl { get; set; } = null!;
    [Required]
    public string Role { get; set; } = "";
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? Linkedin { get; set; }
    public string? Myspace { get; set; }
    public string? Pinterest { get; set; }
    public string? Soundcloud { get; set; }
    public string? Tumblr { get; set; }
    public string? Wikipedia { get; set; }
    public string? Twitter { get; set; }
    public string? Youtube { get; set; }
    public Dictionary<string, string> UserMeta
    {
        get
        {
            return new Dictionary<string, string>
        {
            {"first_name",FirstName},
            {"last_name",LastName},
            {"nickname",Nickname},
            { "facebook", Facebook },
            { "instagram", Instagram },
            { "linkedin", Linkedin },
            { "myspace", Myspace },
            { "pinterest", Pinterest },
            { "soundcloud", Soundcloud },
            { "tumblr", Tumblr },
            { "wikipedia", Wikipedia },
            { "twitter", Twitter },
            { "youtube", Youtube }
        };
        }
    }


}

public class DeleteUserDto
{
    public ulong UserId { get; set; }
    public string UserLogin { get; set; }

}

public class AdminUserDto
{
    public ulong UserId { get; set; }
    public string UserLogin { get; set; }
    public string Role { get; set; }

}