using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using  WP.DTOs;

namespace WP.Data.Repositories
{
    public interface IUserRepository
    {
        Task<WpUser> GetByUsernameAsync(string username);
        Task<bool> CheckEmailExistsAsync(string email);
        Task<bool> CheckUsernameExistsAsync(string username);        
        Task<WpUser> AddUserAsync(WpUser user);
        Task<List<UserDto>> GetAllUsersAsync(Expression<Func<WpUser, bool>> filter = null);
        Task<bool> DeleteUserAsync(List<ulong> Id);
        Task<WpUser> GetUserByEmailAsync(string email);
        Task<WpUser> UpdateUserAsync(WpUser user, EditUserDto userData);  
        Task<WpUser> GetUserById(ulong Id);
        Task<bool> UpdateUserPasswordAsync(WpUser user);
        Task<string> GeneratePasswordResetTokenAsync(WpUser user);
        Task CreateUserAsync(WpUsermetum user);
        Task<string> GetUserRoleAsync(ulong userid);

    }
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

        public async Task<WpUser> AddUserAsync(WpUser user)
        {
            await _dbContext.WpUsers.AddAsync(user);
            return await _dbContext.SaveChangesAsync() > 0 ? user : null;
        }

        public async Task<List<UserDto>> GetAllUsersAsync(Expression<Func<WpUser, bool>> filter = null)
        {
            var query = _dbContext.WpUsers.AsQueryable();
            if(filter != null)
                query = _dbContext.WpUsers.Where(filter);

            return await query.Select(user => new UserDto
            {
                Id = user.Id,
                UserLogin = user.UserLogin,
                UserNicename = user.UserNicename,
                DisplayName = user.DisplayName,
                UserEmail = user.UserEmail,
            }).ToListAsync();
        }

        public async Task<bool> DeleteUserAsync(List<ulong> Ids)
        {
            var usersToDelete = await _dbContext.WpUsers.Where(user => Ids.Contains(user.Id)).ToListAsync();
            if (usersToDelete.Any())
            {
                _dbContext.WpUsers.RemoveRange(usersToDelete);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<WpUser> UpdateUserAsync(WpUser user, EditUserDto userData)
        {
            _dbContext.WpUsers.Update(user);
            await _dbContext.SaveChangesAsync();

            if (!string.IsNullOrEmpty(userData.Role))
            {
                string userLevel = userData.Role switch
                {
                    "administrator" => "10",
                    "editor" => "7",
                    "author" => "2",
                    "contributor" => "1",
                    _ => "0"
                };
                var roleMeta = await _dbContext.WpUsermeta.FirstOrDefaultAsync(m => m.UserId == user.Id && m.MetaKey == "wp_capabilities");
                if (roleMeta != null)
                    roleMeta.MetaValue = $"{{\"{userData.Role}\":true}}";
                else
                    await _dbContext.WpUsermeta.AddAsync(new WpUsermetum { UserId = user.Id, MetaKey = "wp_capabilities", MetaValue = $"{{\"{userData.Role}\":true}}" });

                var levelMeta = await _dbContext.WpUsermeta.FirstOrDefaultAsync(m => m.UserId == user.Id && m.MetaKey == "wp_user_level");
                if (levelMeta != null)
                    levelMeta.MetaValue = userLevel;
                else
                    await _dbContext.WpUsermeta.AddAsync(new WpUsermetum { UserId = user.Id, MetaKey = "wp_user_level", MetaValue = userLevel });

                await _dbContext.SaveChangesAsync();

            }
            var metaList = new List<WpUsermetum>
            {
                new WpUsermetum {
                    UserId = user.Id,
                    MetaKey = "first_name",
                    MetaValue = userData.FirstName
                },
                new WpUsermetum {
                    UserId = user.Id,
                    MetaKey = "last_name",
                    MetaValue = userData.LastName
                },
            };
            //await UpdateUserMeta(user.Id, "first_name", userData.FirstName);
            //await UpdateUserMeta(user.Id, "last_name", userData.LastName);
            //await UpdateUserMeta(user.Id, "nickname", userData.Nickname);
            //await UpdateUserMeta(user.Id, "display_name", userData.DisplayName);
            await UpdateUserMeta(metaList);
            return user;
        }
        private async Task UpdateUserMeta(ulong userId, string metaKey, string metaValue)
        {
            if (string.IsNullOrEmpty(metaValue)) return;

            var meta = await _dbContext.WpUsermeta.FirstOrDefaultAsync(m => m.UserId == userId && m.MetaKey == metaKey);
            if (meta != null)
                meta.MetaValue = metaValue;
            else
                await _dbContext.WpUsermeta.AddAsync(new WpUsermetum { UserId = userId, MetaKey = metaKey, MetaValue = metaValue });

            await _dbContext.SaveChangesAsync();
        }
        private async Task UpdateUserMeta(List<WpUsermetum> collection)
        {
            if(collection.Count > 0)
            {
                foreach (var item in collection)
                {
                    if(_dbContext.WpUsermeta.Any(m => m.UserId == item.UserId && m.MetaKey == item.MetaKey))
                    {
                        _dbContext.Update(item);
                    }
                    else
                    {
                        await _dbContext.WpUsermeta.AddAsync(item);
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task<WpUser> GetUserById(ulong Id)
        {
            return await _dbContext.WpUsers.FirstOrDefaultAsync(user => user.Id == Id);
        }
        public async Task<WpUser> GetUserByEmailAsync(string email)
        {
            return await _dbContext.WpUsers.FirstOrDefaultAsync(user => user.UserEmail == email);
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            var result = await _dbContext.WpUsers.FirstOrDefaultAsync(user => user.UserEmail == email);
            if (result == null)
            {
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

        public async Task CreateUserAsync(WpUsermetum user)
        {
            _dbContext.WpUsermeta.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    
        public async Task<string> GetUserRoleAsync(ulong userid)
        {
            string role = "";
            WpUsermetum umeta = await _dbContext.WpUsermeta.FirstOrDefaultAsync(s=>s.UserId == userid && s.MetaKey == "wp_capabilities");
            if(umeta != null)
            {
                var match = Regex.Match(umeta.MetaValue, @"s:\d+:\""(?<role>[^\""]+)\"";b:(?<value>\d);");
                if (match.Success)
                {
                    role = match.Groups["role"].Value;
                    //bool isAssigned = match.Groups["value"].Value == "1";
                }
            }
            return role;

        }
    }

}
