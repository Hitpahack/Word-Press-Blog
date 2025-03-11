﻿using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using WP.Core;
using WP.Data;
using WP.Data.Repositories;
using WP.DTOs;

namespace WP.Services
{
    public interface IUserService
    {
        Task<ApiResponse<RegisterUserResponseDto>> RegisterUserAsync(UserRegisterDTO userdto);
        Task<ApiResponse<UserResponseDTO>> AuthenticateUserAsync(UserLoginDTO userdto, string Ip);
        Task<ApiResponse<List<UserDto>>> GetAllUsersAsync();
        Task<ApiResponse<string>> DeleteUserAsync(List<ulong> Id);
        Task<ApiResponse<UserDto>> UpdateUserAsync(ulong Id, EditUserDto userData);
        Task<EditUserDto> GetUserByIdAsync(ulong id);
        Task<bool> CheckUserExistsAsync(string username, string email);
        Task<ApiResponse<bool>> SendPasswordResetEmailAsync(ForgotPasswordDTO dto);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDTO dto);
        Task<ApiResponse<RegisterUserResponseDto>> CreateUserAsync(CreateUserDto user);
        Task<ApiResponse<RoleDto>> GetUserRoleAsync(ulong userid);
        Task<ApiResponse<bool>> CheckUsernameExistAsync(string username, ulong userid = 0);
        Task<ApiResponse<bool>> CheckEmailExistAsync(string email, ulong userid = 0);
        Task<DataTableResponse<UserDto>> GetUsersPageAsync(SearchModel filter);
        Task<Dictionary<string, string>> GetUserMetadata(ulong userid);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ILoginAttemptRepository _loginAttemptRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IEmailService emailService, ILoginAttemptRepository loginAttemptRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _loginAttemptRepository = loginAttemptRepository;
            _mapper = mapper;

        }
        public async Task<ApiResponse<RegisterUserResponseDto>> RegisterUserAsync(UserRegisterDTO dto)
        {
            string hashedPassword = PasswordHasher.HashPassword(dto.UserPass);
            WpUser newUser = new WpUser
            {
                UserLogin = dto.UserLogin,
                UserPass = hashedPassword,
                UserEmail = dto.UserEmail,
                UserRegistered = DateTime.UtcNow,
                UserStatus = 1, // Active user
            };
            WpUser result = await _userRepository.AddUserAsync(newUser);
            if (result == null || result.Id == 0)  // Ensure `Id` is generated after insert
            {
                return new FailedApiResponse<RegisterUserResponseDto>("Failed to register user. Please try again.");
            }
            RegisterUserResponseDto response = new RegisterUserResponseDto
            {
                Id = result.Id,
                Username = result.UserLogin,
                Email = result.UserEmail,
            };
            return new SuccessApiResponse<RegisterUserResponseDto>(response, "User registered successfully.");
        }
        public async Task<ApiResponse<UserResponseDTO>> AuthenticateUserAsync(UserLoginDTO dto, string Ip)
        {
            int failedAttempts = await _loginAttemptRepository.GetFailedAttemptsAsync(Ip, dto.Username);
            if (failedAttempts >= 3)
            {
                return new FailedApiResponse<UserResponseDTO>("Too many failed attempts. Try again later.");
            }
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null)
                return new FailedApiResponse<UserResponseDTO>("Username doesn't exist please register first.");

            if (!PasswordHasher.VerifyPassword(dto.Password, user.UserPass))
            {
                await _loginAttemptRepository.AddFailedAttemptAsync(dto.Username, dto.Password, Ip, "Invalid credentials");
                return new FailedApiResponse<UserResponseDTO>("Invalid username or password.");
            }

            await _loginAttemptRepository.ClearFailedAttempts(Ip, dto.Username);

            UserResponseDTO userResponse = new UserResponseDTO
            {
                Id = (int)user.Id,
                Username = user.UserLogin,
            };
            return new SuccessApiResponse<UserResponseDTO>(userResponse, "Login successful");
        }

        public async Task<ApiResponse<List<UserDto>>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            if (users == null || !users.Any())
                return new FailedApiResponse<List<UserDto>>("Users data not found");

            return new SuccessApiResponse<List<UserDto>>(users, "Users retrieved successfully.");
        }

        public async Task<ApiResponse<string>> DeleteUserAsync(List<ulong> Id)
        {
            bool result = await _userRepository.DeleteUserAsync(Id);
            if (result)
                return new SuccessApiResponse<string>("User deleted successfully.", "User deleted successfully.");
            else
                return new FailedApiResponse<string>("User deleted successfully.");
        }

        public async Task<ApiResponse<UserDto>> UpdateUserAsync(ulong Id, EditUserDto userData)
        {
            WpUser user = await _userRepository.GetUserById(Id);
            if (user == null)
            {
                return new ApiResponse<UserDto>(false, "User not found.", null, 404);
            }
            ApiResponse<bool> result = await CheckEmailExistAsync(userData.UserEmail, Id);
            if (result.Data || !result.Success)
                return new FailedApiResponse<UserDto>(result.Message);

            user.UserEmail = userData.UserEmail;
            user.UserUrl = userData.UserUrl ?? "";
            //user.DisplayName = userData.UserLogin;

            WpUser updatedUser = await _userRepository.UpdateUserAsync(user, userData);

            await _userRepository.CreateUserAsync(userData.UserMeta.Select(s => new WpUsermetum
            {
                UserId = Id,
                MetaKey = s.Key,
                MetaValue = s.Value
            }));

            UserDto userDto = _mapper.Map<UserDto>(updatedUser);
            return new SuccessApiResponse<UserDto>(userDto);
        }
      
        public async Task<EditUserDto> GetUserByIdAsync(ulong Id)
        {
            WpUser user = await _userRepository.GetUserById(Id);
            string userrole = await _userRepository.GetUserRoleAsync(Id);
            EditUserDto userDto = _mapper.Map<EditUserDto>(user);
            var userMeta = await _userRepository.GetUserMetaAsync(Id);
            userDto.FirstName = userMeta.TryGetValue("first_name", out var firstName) ? firstName : "";
            userDto.LastName = userMeta.TryGetValue("last_name", out var lastName) ? lastName : "";
            userDto.Nickname = userMeta.TryGetValue("nickname", out var nickname) ? nickname : "";

            //UserDto userDto = _mapper.Map<UserDto>(user);
            userDto.Role = userrole;
            

            if (user == null)
                throw new KeyNotFoundException("User Not Found");
            return userDto;
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
                return new FailedApiResponse<bool>("Email cannot be null or empty.");
            }
            WpUser user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                return new FailedApiResponse<bool>("User with this email does not exist.");
            string token = await _userRepository.GeneratePasswordResetTokenAsync(user);
            string resetLink = $"https://localhost:7084/reset-password.html?token={token}&email={user.UserEmail}";
            string emailBody = $"Click the following link to reset your password: {resetLink}";

            bool result = await _emailService.SendEmailAsync(user.UserEmail, "Password Reset Request", emailBody);
            if (!result)
            {
                return new FailedApiResponse<bool>("Failed to send password reset email.");
            }

            return new SuccessApiResponse<bool>(true, "Password reset email sent successfully.");
        }

        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                return new FailedApiResponse<bool>("Email and new password cannot be empty.");
            }
            WpUser user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
                return new FailedApiResponse<bool>("User not found.");

            // Hash and update the new password
            user.UserPass = PasswordHasher.HashPassword(dto.NewPassword);
            bool result = await _userRepository.UpdateUserPasswordAsync(user);
            if (!result)
            {
                return new FailedApiResponse<bool>("Failed to update the password.");
            }

            return new SuccessApiResponse<bool>(true, "Password reset successfully.");
        }

        public async Task<ApiResponse<RegisterUserResponseDto>> CreateUserAsync(CreateUserDto user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.UserLogin) || string.IsNullOrWhiteSpace(user.UserPass) || string.IsNullOrWhiteSpace(user.UserEmail))
            {
                return new FailedApiResponse<RegisterUserResponseDto>("User data is incomplete.");
            }
            WpUser existingUser = await _userRepository.GetUserByEmailAsync(user.UserEmail);
            if (existingUser != null)
            {
                return new FailedApiResponse<RegisterUserResponseDto>("User with this email already exists.");
            }
            string hashedPassword = PasswordHasher.HashPassword(user.UserPass);
            WpUser newUser = new WpUser
            {
                UserLogin = user.UserLogin,
                UserPass = hashedPassword,
                UserEmail = user.UserEmail,
                DisplayName = $"{user.FirstName} {user.LastName}",
                UserRegistered = DateTime.UtcNow,
                UserStatus = 1,
            };
            WpUser createdUser = await _userRepository.AddUserAsync(newUser);
            if (createdUser == null)
            {
                return new FailedApiResponse<RegisterUserResponseDto>("Failed to create user.");
            }
            var metaList = new List<WpUsermetum>
            {
                new WpUsermetum {
                    UserId = newUser.Id,
                    MetaKey = "first_name",
                    MetaValue = user.FirstName
                },
                new WpUsermetum {
                    UserId = newUser.Id,
                    MetaKey = "last_name",
                    MetaValue = user.LastName
                },
                new WpUsermetum
                {
                    UserId = newUser.Id,
                    MetaKey = "wp_capabilities",
                    MetaValue = $"a:1:{{s:{user.Role.Length}:\"{user.Role}\";b:1;}}" // Serialized PHP format
                },
                new WpUsermetum
                {
                    UserId = newUser.Id,
                    MetaKey = "wp_user_level",
                    MetaValue = user.Role == "administrator" ? "10" : "0"
                }
            };

            await _userRepository.CreateUserAsync(metaList);
            
            //WordPress User Levels(Deprecated but Still Used by Some Plugins)
            // ------------------------------------------------------------
            // Role          | User Level | Capabilities
            // -------------|-----------|-------------------------------------------
            // Administrator | 10        | Full access (manage site, users, settings).
            // Editor       | 7         | Edit, publish, and delete any post.
            // Author       | 2         | Write and publish their own posts.
            // Contributor  | 1         | Write posts, but need approval to publish.
            // Subscriber   | 0         | Read content only (default for new users).


            RegisterUserResponseDto response = new RegisterUserResponseDto
            {
                Id = newUser.Id,
                Email = user.UserEmail,
                Username = user.UserLogin,
            };
            return new SuccessApiResponse<RegisterUserResponseDto>(response, "User created successfully.");
        }

        public async Task<ApiResponse<RoleDto>> GetUserRoleAsync(ulong userid)
        {

            var urole = await _userRepository.GetUserRoleAsync(userid);
            if (urole != null)
            {
                return new SuccessApiResponse<RoleDto>(new RoleDto(userid, urole));
            }
            return new FailedApiResponse<RoleDto>("role doesn't exist");
        }

        public async Task<ApiResponse<bool>> CheckUsernameExistAsync(string username, ulong userid = 0)
        {
            var predicate = PredicateBuilder.True<WpUser>().And(s => s.UserLogin.ToLower() == username.ToLower());
            if (userid > 0)
                predicate = predicate.And(s => s.Id != userid);
            var urole = await _userRepository.GetAllUsersAsync(predicate);
            return new SuccessApiResponse<bool>(urole.Count > 0);
        }
        public async Task<ApiResponse<bool>> CheckEmailExistAsync(string email, ulong userid = 0)
        {
            var predicate = PredicateBuilder.True<WpUser>().And(s => s.UserEmail.ToLower() == email.ToLower());
            if (userid > 0)
                predicate = predicate.And(s => s.Id != userid);

            var urole = await _userRepository.GetAllUsersAsync(predicate);
            return new SuccessApiResponse<bool>(urole.Count > 0);
        }
        public async Task<DataTableResponse<UserDto>> GetUsersPageAsync(SearchModel filter)
        {
            var users = await _userRepository.GetUsersPageAsync(filter);
            return users;
        }
        public async Task<Dictionary<string, string>> GetUserMetadata(ulong userid)
        {
            var metadata = await _userRepository.GetUserMetaAsync(userid);
            return metadata;
        }
    }
}
