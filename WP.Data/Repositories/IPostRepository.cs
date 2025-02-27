using Microsoft.EntityFrameworkCore;
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
    public class PostRepository : IPostRepository
    {
        private readonly BlogContext _dbContext;

        public PostRepository(BlogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreatePostAsync(WpPost post)
        {
            await _dbContext.WpPosts.AddAsync(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<PostDto>> GetAllPostAsync()
        {
            return await _dbContext.WpPosts.Where(post => post.PostType == "post").Select(post => new PostDto
            {
                Id = post.Id,
                CommentCount = post.CommentCount,
                Guid = post.Guid,
                CommentStatus = post.CommentStatus,
                MenuOrder = post.MenuOrder,
                Pinged = post.Pinged,
                PingStatus = post.PingStatus,
                PostAuthor = post.PostAuthor,
                PostContent = post.PostContent,
                PostTitle = post.PostTitle,
                PostContentFiltered = post.PostContentFiltered,
                PostDate = post.PostDate,
                PostDateGmt = post.PostDateGmt,
                PostExcerpt = post.PostExcerpt,
                PostMimeType = post.PostMimeType,
                PostModified = post.PostModified,
                PostModifiedGmt = post.PostModifiedGmt,
                PostName = post.PostName,
                PostParent = post.PostParent,
                PostPassword = post.PostPassword,
                PostStatus = post.PostStatus,
                PostType = post.PostType,
                ToPing = post.ToPing,
            }).ToListAsync();
        }

        public async Task DeletePostAsync(List<ulong> Ids)
        {
            var postToDelete = await _dbContext.WpPosts.Where(post => Ids.Contains(post.Id)).ToListAsync();
            if (postToDelete.Any())
            {
                _dbContext.WpPosts.RemoveRange(postToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdatePostAsync(WpPost post)
        {
            _dbContext.WpPosts.Update(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<WpPost> GetPostByIdAsync(ulong Id)
        {
            return await _dbContext.WpPosts.FirstOrDefaultAsync(post => post.Id == Id);
        }

        public async Task<WpPost> GetPostByNameAsync(string postName)
        {
            return await _dbContext.WpPosts.FirstOrDefaultAsync(post => post.PostTitle == postName);
        }
    }

}
