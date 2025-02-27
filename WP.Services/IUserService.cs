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

namespace WP.Services
{
    public interface IUserService 
    {
        Task<bool> RegisterUserAsync(UserRegisterDTO userdto);
        Task<UserResponseDTO> AuthenticateUserAsync(UserLoginDTO userdto,string Ip);
        Task<List<UserDto>> GetAllUsersAsync(); 
        Task DeleteUserAsync(List<ulong> Id);
        Task UpdateUserAsync(WpUser userdto);
        Task<WpUser> GetUserByIdAsync(ulong id);
        Task<bool> CheckUserExistsAsync(string username,string email);
        Task<bool> SendPasswordResetEmailAsync(ForgotPasswordDTO dto);
        Task<bool> ResetPasswordAsync(ResetPasswordDTO dto);
        Task<WpUser> CreateUserAsync(CreateUserDto user);
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
        public async Task<bool> RegisterUserAsync(UserRegisterDTO dto)
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
            await _userRepository.AddUserAsync(newUser);
            return true;
        }
        public async Task<UserResponseDTO> AuthenticateUserAsync(UserLoginDTO dto, string Ip)
        {
            int failedAttempts = await _loginAttemptRepository.GetFailedAttemptsAsync(Ip, dto.Username);
            if (failedAttempts >= 3)
            {
                return new UserResponseDTO { Message = "Too many failed attempts. Try again later.", Id = 0, Username = null };
            }
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null || !PasswordHasher.VerifyPassword(dto.Password, user.UserPass))
            {
                await _loginAttemptRepository.AddFailedAttemptAsync(dto.Username, dto.Password, Ip, "Invalid credentials");
                return new UserResponseDTO { Message = "Invalid username or password.", Id = 0, Username = null };
            }

            await _loginAttemptRepository.ClearFailedAttempts(Ip, dto.Username);
            return new UserResponseDTO { Message = "Login succussfully.", Id = (int)user.Id, Username = user.UserNicename };
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users;
        }

        public async Task DeleteUserAsync(List<ulong> Id)
        {
            await _userRepository.DeleteUserAsync(Id);
        }

        public async Task UpdateUserAsync(WpUser userdto)
        {
            var user = await _userRepository.GetByUsernameAsync(userdto.UserNicename);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            user.UserEmail = userdto.UserEmail;
            user.UserPass = userdto.UserPass;
            user.UserUrl = userdto.UserUrl;
            user.DisplayName = userdto.DisplayName;
            await _userRepository.UpdateUserAsync(userdto);
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

        public async Task<bool> SendPasswordResetEmailAsync(ForgotPasswordDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(dto.Email));
            }
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                return false;
            string token = await _userRepository.GeneratePasswordResetTokenAsync(user);
            string resetLink = $"https://localhost:7084/reset-password.html?token={token}&email={user.UserEmail}";
            string emailBody = $"Click the following link to reset your password: {resetLink}";

            return await _emailService.SendEmailAsync(user.UserEmail, "Password Reset Request", emailBody);
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                return false;
            user.UserPass = PasswordHasher.HashPassword(dto.NewPassword);
            return await _userRepository.UpdateUserPasswordAsync(user);
        }

        public async Task<WpUser> CreateUserAsync(CreateUserDto user)
        {
            var hashedPassword = PasswordHasher.HashPassword(user.UserPass);
            var newUser = new WpUser
            {
                UserLogin = user.UserLogin,
                UserPass = hashedPassword,
                UserEmail = user.UserEmail,
                DisplayName = user.FirstName + " " + user.LastName,
                UserRegistered = DateTime.UtcNow,
                UserStatus = 1,
            };
            await _userRepository.AddUserAsync(newUser);

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
            return newUser;
        }
    }
}
