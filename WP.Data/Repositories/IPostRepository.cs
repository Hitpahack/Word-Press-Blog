using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.DTOs;

namespace WP.Data.Repositories
{
    public interface IPostRepository
    {
        Task<WpPost> GetPostByIdAsync(ulong Id);
        Task<WpPost> GetPostByNameAsync(string postName);
        Task<IEnumerable<PostDto>> GetAllPostAsync();
        Task CreatePostAsync(WpPost post);
        Task DeletePostAsync(List<ulong> Id);
        Task UpdatePostAsync(WpPost post);
    }
}
