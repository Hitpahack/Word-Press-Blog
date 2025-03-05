using Microsoft.EntityFrameworkCore;
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
        Task<IEnumerable<PageDto>> GetAllPageAsync();
        Task<PageDto> GetPageByIdAsync(ulong Id);
        Task<WpPost> GetPageByNameAsync(string pageName);
        Task<WpPost> CreatePageAsync(CreatePageDto page);
        Task<bool> DeletePageAsync(List<ulong> Id);
        Task<bool> UpdatePageAsync(ulong Id , UpdatePageDto page);
    }
    public class PageRepository : IPageRepository
    {
        private readonly BlogContext _dbContext;

        public PageRepository(BlogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PageDto>> GetAllPageAsync()
        {
            return await _dbContext.WpPosts
            .Where(p => p.PostType == "page" && p.PostStatus != "trash")
            .Select(p => new PageDto
            {
                Id = p.Id,
                Title = p.PostTitle,
                Content = p.PostContent,
                Excerpt = p.PostExcerpt,
                Status = p.PostStatus,
                CreatedAt = p.PostDate,
                AuthorId = p.PostAuthor,
                ParentId = p.PostParent
            })
            .ToListAsync();
        }
        public async Task<PageDto> GetPageByIdAsync(ulong Id)
        {
            return await _dbContext.WpPosts
            .Where(p => p.Id == Id && p.PostType == "page")
            .Select(p => new PageDto
            {
                Id = p.Id,
                Title = p.PostTitle,
                Content = p.PostContent,
                Excerpt = p.PostExcerpt,
                Status = p.PostStatus,
                CreatedAt = p.PostDate,
                AuthorId = p.PostAuthor,
                ParentId = p.PostParent
            })
            .FirstOrDefaultAsync();
        }
        public async Task<WpPost> CreatePageAsync(CreatePageDto pageDto)
        {
            var page = new WpPost
            {
                PostAuthor = pageDto.AuthorId,
                PostTitle = pageDto.Title,
                PostContent = pageDto.Content,
                PostExcerpt = pageDto.Excerpt,
                PostStatus = pageDto.Status,
                PostName = pageDto.Title.ToLower().Replace(" ", "-"),
                PostType = "page",
                PostParent = pageDto.ParentId
            };

            _dbContext.WpPosts.Add(page);
            await _dbContext.SaveChangesAsync();
            return page;
        }

        public async Task<WpPost> GetPageByNameAsync(string pageName)
        {
            return await _dbContext.WpPosts.FirstOrDefaultAsync(page => page.PostTitle == pageName);
        }
        public async Task<bool> DeletePageAsync(List<ulong> Ids)
        {
            var pageToDelete = await _dbContext.WpPosts.Where(post => Ids.Contains(post.Id)).ToListAsync();
            if (pageToDelete.Any())
            {
                _dbContext.WpPosts.RemoveRange(pageToDelete);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> UpdatePageAsync(ulong Id, UpdatePageDto pageDto)
        {
            var page = await _dbContext.WpPosts.FindAsync(Id);
            if (page == null || page.PostType != "page") return false;

            page.PostTitle = pageDto.Title ?? page.PostTitle;
            page.PostContent = pageDto.Content ?? page.PostContent;
            page.PostExcerpt = pageDto.Excerpt ?? page.PostExcerpt;
            page.PostStatus = pageDto.Status ?? page.PostStatus;
            page.PostParent = pageDto.ParentId;

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }

}
