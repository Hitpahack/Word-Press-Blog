using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  WP.DTOs;

namespace WP.Data.Repositories
{
    public interface IUserRepository
    {
        Task<WpUser> GetByUsernameAsync(string username);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckUsernameExistsAsync(string username);        
        Task AddUserAsync(WpUser user);
        Task<List<UserDto>> GetAllUsersAsync();
        Task DeleteUserAsync(List<ulong> Id);
        Task<WpUser> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(WpUser user);  
        Task<WpUser> GetUserById(ulong Id);
        Task<bool> UpdateUserPasswordAsync(WpUser user);
        Task<string> GeneratePasswordResetTokenAsync(WpUser user);

    }
}
