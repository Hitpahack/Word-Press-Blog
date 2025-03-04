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
    public interface ICategoryService
    {
        Task<ApiResponse<IEnumerable<CategoryResponseDto>>> GetAllCategoryAsync();
        Task<ApiResponse<CategoryRequestDto>> AddCategoryAsync(CategoryRequestDto category);
        Task<ApiResponse<string>> DeleteCategoryAsync(List<ulong> Ids);
        Task<ApiResponse<string>> QuickUpdateCategoryAsync(WpTerm category);
        Task<ApiResponse<bool>> UpdateCategoryAsync(CategoryDto category);

    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository repository)
        {
            _categoryRepository = repository;
        }

        public async Task<ApiResponse<CategoryRequestDto>> AddCategoryAsync(CategoryRequestDto category)
        {
            var createdCategory = await _categoryRepository.AddCategoryAsync(category);

            if (createdCategory == null)
            {
                return new FailedApiResponse<CategoryRequestDto>("Failed to add category.");
            }

            return new SuccessApiResponse<CategoryRequestDto>(createdCategory, "Category added successfully.");
        }

        public async Task<ApiResponse<string>> DeleteCategoryAsync(List<ulong> Ids)
        {
            await _categoryRepository.DeleteCategoryAsync(Ids);
            return new SuccessApiResponse<string>("Categories deleted successfully.");
        }

        public async Task<ApiResponse<IEnumerable<CategoryResponseDto>>> GetAllCategoryAsync()
        {
            var categories = await _categoryRepository.GetAllCategoryAsync();

            if (categories == null || !categories.Any())
            {
                return new FailedApiResponse<IEnumerable<CategoryResponseDto>>("No categories found.");
            }

            return new SuccessApiResponse<IEnumerable<CategoryResponseDto>>(categories, "Categories retrieved successfully.");
        }

        public async Task<ApiResponse<string>> QuickUpdateCategoryAsync(WpTerm category)
        {
            bool updated = await _categoryRepository.QuickUpdateCategoryAsync(category);
            if (!updated)
            {
                return new FailedApiResponse<string>("Failed to update category.");
            }

            return new SuccessApiResponse<string>("Category updated successfully.");
        }

        public async Task<ApiResponse<bool>> UpdateCategoryAsync(CategoryDto category)
        {
            bool updated = await _categoryRepository.UpdateCategoryAsync(category);
            if (!updated)
            {
                return new FailedApiResponse<bool>("Failed to update category.");
            }

            return new SuccessApiResponse<bool>(true,"Category updated successfully.");
        }
    }
}
        