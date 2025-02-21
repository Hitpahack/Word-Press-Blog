using WP.Data.Repositories;
using static WP.DTOs.UserDtos;
using WP.Data;
using WP.Core;

namespace WP.Services;

public class UserService : IUserService
{
	private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
	public async Task<UserResponseDTO> AuthenticateUserAsync(UserLoginDTO dto)
	{
		var user = await _userRepository.GetByUsernameAsync(dto.Username);
		if (user == null || !PasswordHasher.VerifyPassword(dto.Password, user.UserPass))
			return null; // Invalid credentials

		return new UserResponseDTO { Id = (int)user.Id, Username = user.UserNicename };
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
		if(user == null)
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
}
    