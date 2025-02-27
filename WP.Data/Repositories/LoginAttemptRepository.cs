using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.Data.Repositories
{
    public class LoginAttemptRepository :ILoginAttemptRepository
    {
        private readonly BlogContext _dbContext;

        public LoginAttemptRepository(BlogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddFailedAttemptAsync(string username, string password, string ip, string reason)
        {
            ulong userId = await _dbContext.WpUsers.Where(user => user.UserLogin == username).Select(user => user.Id).FirstOrDefaultAsync();

            var failedAttempt = new WpWpcLoginFail
            {
                UserId = (long)userId,
                FailedUser = username,
                FailedPass = password,
                LoginAttemptDate = DateTime.UtcNow,
                Reason = reason,
                LoginAttemptIp = ip,
            };
            await _dbContext.WpWpcLoginFails.AddAsync(failedAttempt);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ClearFailedAttempts(string ip, string username)
        {
            var attempts = _dbContext.WpWpcLoginFails.Where(a => a.LoginAttemptIp == ip && a.FailedUser == username);
            _dbContext.WpWpcLoginFails.RemoveRange(attempts);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetFailedAttemptsAsync(string ip, string username)
        {

            var last15Min = DateTime.UtcNow.AddMinutes(-15);
            return await _dbContext.WpWpcLoginFails.Where(data => data.LoginAttemptIp == ip && data.FailedUser == username && data.LoginAttemptDate>=last15Min).CountAsync();
        }
    }
}
