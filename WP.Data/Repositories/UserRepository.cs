using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  WP.DTOs;

namespace WP.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BlogContext _dbContext;

        public UserRepository(BlogContext context)
        {
            _dbContext = context;
        }

        public async Task<WpUser> GetByUsernameAsync(string username)
        {
            return await _dbContext.WpUsers.FirstOrDefaultAsync(u => u.UserLogin == username);
        }

        public async Task AddUserAsync(WpUser user)
        {
            await _dbContext.WpUsers.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _dbContext.WpUsers.Select(user => new UserDto
            {
                Id=user.Id,
                UserLogin=user.UserLogin,
                UserNicename = user.UserNicename,
                DisplayName = user.DisplayName,
                UserEmail = user.UserEmail,
            }).ToListAsync();
        }

        public async Task DeleteUserAsync(List<ulong> Ids)
        {
            var usersToDelete = await _dbContext.WpUsers.Where(user => Ids.Contains(user.Id)).ToListAsync();
            if (usersToDelete.Any()) {
                _dbContext.WpUsers.RemoveRange(usersToDelete);
                await _dbContext.SaveChangesAsync();              
            }
        }

        public async Task UpdateUserAsync(WpUser user)
        {
            _dbContext.WpUsers.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<WpUser> GetUserById(ulong Id)
        {
            return  await _dbContext.WpUsers.FirstOrDefaultAsync(user => user.Id == Id);
        }
        public async Task<WpUser> GetUserByEmailAsync(string email)
        {
            return await _dbContext.WpUsers.FirstOrDefaultAsync(user => user.UserEmail == email);
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            var result = await _dbContext.WpUsers.FirstOrDefaultAsync(user => user.UserEmail == email);
            if (result == null) { 
                return false;
            }
            return true;
        }

        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            var result = await _dbContext.WpUsers.FirstOrDefaultAsync(user => user.UserLogin == username);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateUserPasswordAsync(WpUser user)
        {
            _dbContext.WpUsers.Update(user);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(WpUser user)
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
