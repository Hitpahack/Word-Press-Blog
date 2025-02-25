using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.Data.Repositories
{
    public interface ILoginAttemptRepository
    {
        Task<int> GetFailedAttemptsAsync(string ip, string username);
        Task AddFailedAttemptAsync(string username, string password, string ip, string reason);
        Task ClearFailedAttempts(string ip, string username);

    }
}
