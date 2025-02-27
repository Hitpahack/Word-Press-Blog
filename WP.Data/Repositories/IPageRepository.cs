using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.DTOs;

namespace WP.Data.Repositories
{
    public interface IPageRepository
    {
        Task<WpPost> GetPageByIdAsync(ulong Id);
        Task<WpPost> GetPageByNameAsync(string pageName);
        Task<IEnumerable<PageDto>> GetAllPageAsync();
        Task CreatePageAsync(WpPost page);
        Task DeletePageAsync(List<ulong> Id);
        Task UpdatePageAsync(WpPost page);
    }
}
