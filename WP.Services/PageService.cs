using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.Data;
using WP.Data.Repositories;
using WP.DTOs;

namespace WP.Services
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;  
        }

        public async Task<bool> CreatePageAsync(WpPost page)
        {
            var newPage = new WpPost
            {
                Id = page.Id,
                CommentCount = page.CommentCount,
                CommentStatus = page.CommentStatus,
                Guid = page.Guid,
                MenuOrder = page.MenuOrder,
                Pinged = page.Pinged,
                PingStatus = page.PingStatus,
                PostAuthor = page.PostAuthor,
                PostContent = page.PostContent,
                PostContentFiltered = page.PostContentFiltered,
                PostDate = page.PostDate,
                PostDateGmt = page.PostDateGmt,
                PostExcerpt = page.PostExcerpt,
                PostMimeType = page.PostMimeType,
                PostModified = page.PostModified,
                PostTitle = page.PostTitle,
                PostModifiedGmt = page.PostModifiedGmt,
                PostName = page.PostName,
                PostParent = page.PostParent,
                PostStatus = page.PostStatus,
                PostPassword = page.PostPassword,
                PostType = page.PostType,
                ToPing = page.ToPing,
            };

            await _pageRepository.CreatePageAsync(newPage);
            return true;
        }

        public async Task DeletePageAsync(List<ulong> Ids)
        {
            await _pageRepository.DeletePageAsync(Ids);
        }

        public async Task<IEnumerable<PageDto>> GetAllPageAsync()
        {
            var pages = await _pageRepository.GetAllPageAsync();
            return pages;
        }

        public async Task<WpPost> GetPageByNameAsync(string pageTitle)
        {
            var page = await _pageRepository.GetPageByNameAsync(pageTitle);
            if (page != null)
            {
                return page;
            }
            return null;
        }

        public async Task UpdatePageAsync(WpPost page)
        {
            var existingpage = await _pageRepository.GetPageByIdAsync(page.Id);
            if (existingpage == null)
            {
                throw new KeyNotFoundException("Post not found.");
            }
            existingpage.CommentCount = page.CommentCount;
            existingpage.CommentStatus = page.CommentStatus;
            existingpage.Guid = page.Guid;
            existingpage.MenuOrder = page.MenuOrder;
            existingpage.Pinged = page.Pinged;
            existingpage.PingStatus = page.PingStatus;
            existingpage.PostAuthor = page.PostAuthor;
            existingpage.PostContent = page.PostContent;
            existingpage.PostContentFiltered = page.PostContentFiltered;
            existingpage.PostDate = page.PostDate;
            existingpage.PostDateGmt = page.PostDateGmt;
            existingpage.PostExcerpt = page.PostExcerpt;
            existingpage.PostMimeType = page.PostMimeType;
            existingpage.PostModified = page.PostModified;
            existingpage.PostTitle = page.PostTitle;
            existingpage.PostModifiedGmt = page.PostModifiedGmt;
            existingpage.PostName = page.PostName;
            existingpage.PostParent = page.PostParent;
            existingpage.PostStatus = page.PostStatus;
            existingpage.PostPassword = page.PostPassword;
            existingpage.PostType = page.PostType;
            existingpage.ToPing = page.ToPing;
            await _pageRepository.UpdatePageAsync(existingpage);
        }
    }
}
