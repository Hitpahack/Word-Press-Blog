using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.Data;
using static WP.DTOs.UserDtos;

namespace WP.Services
{
    public interface IUserService 
    {
        Task<bool> RegisterUserAsync(UserRegisterDTO userdto);
        Task<UserResponseDTO> AuthenticateUserAsync(UserLoginDTO userdto);
        Task<List<UserDto>> GetAllUsersAsync(); 
        Task DeleteUserAsync(List<ulong> Id);
        Task UpdateUserAsync(WpUser userdto);
        Task<WpUser> GetUserByIdAsync(ulong id);
        Task<bool> CheckUserExistsAsync(string username,string email);
    }
}
