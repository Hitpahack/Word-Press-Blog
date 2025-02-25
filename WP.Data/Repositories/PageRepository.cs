using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.DTOs;

namespace WP.Data.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly BlogContext _dbContext;

        public PageRepository(BlogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreatePageAsync(WpPost page)
        {
            await _dbContext.WpPosts.AddAsync(page);
            await _dbContext.SaveChangesAsync();
        }
            
        public async Task DeletePageAsync(List<ulong> Ids)
        {
            var pageToDelete = await _dbContext.WpPosts.Where(post => Ids.Contains(post.Id)).ToListAsync();
            if (pageToDelete.Any())
            {
                _dbContext.WpPosts.RemoveRange(pageToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PageDto>> GetAllPageAsync()
        {
            return await _dbContext.WpPosts.Select(post => new PageDto
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
        public async Task<WpPost> GetPageByIdAsync(ulong Id)
        {
            return await _dbContext.WpPosts.FirstOrDefaultAsync(page => page.Id == Id);
        }

        public async Task<WpPost> GetPageByNameAsync(string pageName)
        {
            return await _dbContext.WpPosts.FirstOrDefaultAsync(page => page.PostTitle == pageName);
        }

        public async Task UpdatePageAsync(WpPost page)
        {

            _dbContext.WpPosts.Update(page);
            await _dbContext.SaveChangesAsync();
        }
    }
}
