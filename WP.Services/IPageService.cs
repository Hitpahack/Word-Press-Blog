using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.Data;
using WP.DTOs;

namespace WP.Services
{
    public interface IPageService
    {
        Task<IEnumerable<PageDto>> GetAllPageAsync();
        Task<bool> CreatePageAsync(WpPost page);
        Task DeletePageAsync(List<ulong> Ids);
        Task UpdatePageAsync(WpPost page);
        Task<WpPost> GetPageByNameAsync(string pageTitle);
    }
}
