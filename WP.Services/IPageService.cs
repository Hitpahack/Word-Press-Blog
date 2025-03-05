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
    public interface IPageService
    {
        Task<ApiResponse<IEnumerable<PageDto>>> GetAllPageAsync();
        Task<ApiResponse<WpPost>> CreatePageAsync(CreatePageDto page);
        Task<ApiResponse<bool>> DeletePageAsync(List<ulong> Ids);
        Task<ApiResponse<bool>> UpdatePageAsync(ulong Id, UpdatePageDto page);
        Task<ApiResponse<WpPost>> GetPageByNameAsync(string pageTitle);
    }
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public async Task<ApiResponse<WpPost>> CreatePageAsync(CreatePageDto page)
        {
            WpPost result=await _pageRepository.CreatePageAsync(page);
            if(result == null)
                return new FailedApiResponse<WpPost>("Failed to create page");

            return new SuccessApiResponse<WpPost>(result, "Page created sucessfully");
        }

        public async Task<ApiResponse<bool>> DeletePageAsync(List<ulong> Ids)
        {
            bool result = await _pageRepository.DeletePageAsync(Ids);
            if(!result)
                return new FailedApiResponse<bool>("Failed to delete page");

            return new SuccessApiResponse<bool>(true, "Page deleted sucessfully");
        }

        public async Task<ApiResponse<IEnumerable<PageDto>>> GetAllPageAsync()
        {
            var pages = await _pageRepository.GetAllPageAsync();
            if (pages == null || pages.Any())
                return new FailedApiResponse<IEnumerable<PageDto>>("Failed to get pages data");
            return new SuccessApiResponse<IEnumerable<PageDto>>(pages, "Page data sucessfully retrived");

        }

        public async Task<ApiResponse<WpPost>> GetPageByNameAsync(string pageTitle)
        {
            WpPost page = await _pageRepository.GetPageByNameAsync(pageTitle);
            if (page != null)
                return new FailedApiResponse<WpPost>("Failed to get page data");

            return new SuccessApiResponse<WpPost>(page, "Page data sucessfully retrived");
        }

        public async Task<ApiResponse<bool>> UpdatePageAsync(ulong Id, UpdatePageDto page)
        {
            var existingpage = await _pageRepository.GetPageByIdAsync(Id);
            if (existingpage == null)
            {
                return new FailedApiResponse<bool>("Post not found.");
            }
            bool result = await _pageRepository.UpdatePageAsync(Id,page);
            if(result)
                return new FailedApiResponse<bool>("Failed to update page data");

            return new SuccessApiResponse<bool>(true, "Page data sucessfully updated");
        }
    }

}
