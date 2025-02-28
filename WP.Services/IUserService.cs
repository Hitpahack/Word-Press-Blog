using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.Core;
using WP.Data;
using WP.Data.Repositories;
using  WP.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WP.Services
{
    public interface IUserService 
    {
        Task<ApiResponse<RegisterUserResponseDto>> RegisterUserAsync(UserRegisterDTO userdto);
        Task<ApiResponse<UserResponseDTO>> AuthenticateUserAsync(UserLoginDTO userdto,string Ip);
        Task<ApiResponse<List<UserDto>>> GetAllUsersAsync();
        Task<ApiResponse<string>> DeleteUserAsync(List<ulong> Id);
        Task<ApiResponse<string>> UpdateUserAsync(ulong Id, UpdateUserDto userData);
        Task<WpUser> GetUserByIdAsync(ulong id);
        Task<bool> CheckUserExistsAsync(string username,string email);
        Task<ApiResponse<bool>> SendPasswordResetEmailAsync(ForgotPasswordDTO dto);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDTO dto);
        Task<ApiResponse<RegisterUserResponseDto>> CreateUserAsync(CreateUserDto user);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ILoginAttemptRepository _loginAttemptRepository;

        public UserService(IUserRepository userRepository, IEmailService emailService, ILoginAttemptRepository loginAttemptRepository)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _loginAttemptRepository = loginAttemptRepository;
        }
        public async Task<ApiResponse<RegisterUserResponseDto>> RegisterUserAsync(UserRegisterDTO dto)
        {
            var hashedPassword = PasswordHasher.HashPassword(dto.UserPass);
            var newUser = new WpUser
            {
                UserLogin = dto.UserLogin,
                UserPass = hashedPassword,
                UserEmail = dto.UserEmail,
                UserRegistered = DateTime.UtcNow,
                UserStatus = 1, // Active user
            };
            var result = await _userRepository.AddUserAsync(newUser);
            if (result == null || result.Id == 0)  // Ensure `Id` is generated after insert
            {
                return new ApiResponse<RegisterUserResponseDto>(false, "Failed to register user. Please try again.", null, 500);
            }
            var response = new RegisterUserResponseDto
            {
                Id = result.Id,
                Username = result.UserLogin,
                Email = result.UserEmail,
            };
            return new ApiResponse<RegisterUserResponseDto>(true, "User registered successfully.", response, 201);
        }
        public async Task<ApiResponse<UserResponseDTO>> AuthenticateUserAsync(UserLoginDTO dto, string Ip)
        {
            int failedAttempts = await _loginAttemptRepository.GetFailedAttemptsAsync(Ip, dto.Username);
            if (failedAttempts >= 3)
            {
                return new ApiResponse<UserResponseDTO>(false, "Too many failed attempts. Try again later.", null, 429);
            }
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null || !PasswordHasher.VerifyPassword(dto.Password, user.UserPass))
            {
                await _loginAttemptRepository.AddFailedAttemptAsync(dto.Username, dto.Password, Ip, "Invalid credentials");
                return new ApiResponse<UserResponseDTO>(false, "Invalid username or password.", null, 401);
            }

            await _loginAttemptRepository.ClearFailedAttempts(Ip, dto.Username);

            var userResponse = new UserResponseDTO
            {
                Id = (int)user.Id,
                Username = user.UserLogin,
            };
            return new ApiResponse<UserResponseDTO>(true, "Login successful.", userResponse, 200);
        }

        public async Task<ApiResponse<List<UserDto>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            if (users == null || !users.Any())
                return new ApiResponse<List<UserDto>>(false, "Users data not found", null, 404);

            return new ApiResponse<List<UserDto>>(true, "Users retrieved successfully.", users, 200);
        }

        public async Task<ApiResponse<string>> DeleteUserAsync(List<ulong> Id)
        {
            bool result = await _userRepository.DeleteUserAsync(Id);
            return new ApiResponse<string>(result, result ? "User deleted successfully." : "User not found.", null, result ? 200 : 404);
        }

        public async Task<ApiResponse<string>> UpdateUserAsync(ulong Id, UpdateUserDto userData)
        {
            var user = await _userRepository.GetUserById(Id);
            if (user == null)
            {
                return new ApiResponse<string>(false, "User not found.",null, 404);
            }
            bool existingEmail = await _userRepository.CheckEmailExistsAsync(userData.Email);
            if(existingEmail)
                return new ApiResponse<string>(false, "Email already exists.", null, 404);

            var hashedPassword = PasswordHasher.HashPassword(userData.Password);

            user.UserEmail = userData.Email;
            user.UserPass = userData.Password;
            user.UserUrl = userData.UserUrl;
            user.UserPass = hashedPassword;
            user.DisplayName = userData.DisplayName;
            
            bool updatedUser = await _userRepository.UpdateUserAsync(user,userData);
            return new ApiResponse<string>(updatedUser,updatedUser ? "User updated successfully." : "Failed to update user.", null, updatedUser ? 200 : 500);
        }

        public async Task<WpUser> GetUserByIdAsync(ulong Id)
        {
            var user = await _userRepository.GetUserById(Id);
            if (user == null)
                throw new KeyNotFoundException("User Not Found");
            return user;
        }

        public async Task<bool> CheckUserExistsAsync(string username, string email)
        {
            bool existingUser = await _userRepository.CheckUsernameExistsAsync(username);
            bool existingEmail = await _userRepository.CheckEmailExistsAsync(email);
            if (!existingUser && !existingEmail)
            {
                return false;
            }
            return true;
        }

        public async Task<ApiResponse<bool>> SendPasswordResetEmailAsync(ForgotPasswordDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email))
            {
                return new ApiResponse<bool>(false, "Email cannot be null or empty.", false, 400);
            }
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                return new ApiResponse<bool>(false, "User with this email does not exist.", false, 404);
            string token = await _userRepository.GeneratePasswordResetTokenAsync(user);
            string resetLink = $"https://localhost:7084/reset-password.html?token={token}&email={user.UserEmail}";
            string emailBody = $"Click the following link to reset your password: {resetLink}";

            bool result = await _emailService.SendEmailAsync(user.UserEmail, "Password Reset Request", emailBody);
            if (!result)
            {
                return new ApiResponse<bool>(false, "Failed to send password reset email.", false, 500);
            }

            return new ApiResponse<bool>(true, "Password reset email sent successfully.", true, 200);
        }

        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                return new ApiResponse<bool>(false, "Email and new password cannot be empty.", false, 400);
            }
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                return new ApiResponse<bool>(false, "User not found.", false, 404);

            // Hash and update the new password
            user.UserPass = PasswordHasher.HashPassword(dto.NewPassword);
            bool result = await _userRepository.UpdateUserPasswordAsync(user);
            if (!result)
            {
                return new ApiResponse<bool>(false, "Failed to update the password.", false, 500);
            }

            return new ApiResponse<bool>(true, "Password reset successfully.", true, 200);
        }

        public async Task<ApiResponse<RegisterUserResponseDto>> CreateUserAsync(CreateUserDto user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.UserLogin) || string.IsNullOrWhiteSpace(user.UserPass) || string.IsNullOrWhiteSpace(user.UserEmail))
            {
                return new ApiResponse<RegisterUserResponseDto>(false, "User data is incomplete.", null, 400);
            }
            var existingUser = await _userRepository.GetUserByEmailAsync(user.UserEmail);
            if (existingUser != null)
            {
                return new ApiResponse<RegisterUserResponseDto>(false, "User with this email already exists.", null, 409);
            }
            var hashedPassword = PasswordHasher.HashPassword(user.UserPass);
            var newUser = new WpUser
            {
                UserLogin = user.UserLogin,
                UserPass = hashedPassword,
                UserEmail = user.UserEmail,
                DisplayName = $"{user.FirstName} {user.LastName}",
                UserRegistered = DateTime.UtcNow,
                UserStatus = 1,
            };
            var createdUser = await _userRepository.AddUserAsync(newUser);
            if (createdUser == null)
            {
                return new ApiResponse<RegisterUserResponseDto>(false, "Failed to create user.", null, 500);
            }

            var userMeta1 = new WpUsermetum
            {
                UserId = newUser.Id,
                MetaKey = "wp_capabilities",
                MetaValue = $"a:1:{{s:{user.Role.Length}:\"{user.Role}\";b:1;}}" // Serialized PHP format
            };

            await _userRepository.CreateUserAsync(userMeta1);
            var userMeta2 = new WpUsermetum
            {
                UserId = newUser.Id,
                MetaKey = "wp_user_level",
                MetaValue = user.Role == "administrator" ? "10" : "0"
            };
            //WordPress User Levels(Deprecated but Still Used by Some Plugins)
            // ------------------------------------------------------------
            // Role          | User Level | Capabilities
            // -------------|-----------|-------------------------------------------
            // Administrator | 10        | Full access (manage site, users, settings).
            // Editor       | 7         | Edit, publish, and delete any post.
            // Author       | 2         | Write and publish their own posts.
            // Contributor  | 1         | Write posts, but need approval to publish.
            // Subscriber   | 0         | Read content only (default for new users).

            await _userRepository.CreateUserAsync(userMeta2);

            var response = new RegisterUserResponseDto
            {
                Id = newUser.Id,
                Email = user.UserEmail,
                Username = user.UserLogin,
            };
            return new ApiResponse<RegisterUserResponseDto>(true, "User created successfully.", response, 201);
        }
    }
}
