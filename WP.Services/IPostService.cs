using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.Data;
using WP.DTOs;

namespace WP.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPostsAsync();
        Task<bool> CreatePostAsync(WpPost post);
        Task DeletePostAsync(List<ulong> Ids);  
        Task UpdatePostAsync(WpPost post);
        Task<WpPost> GetPostByNameAsync(string postTitle);
    }
}
