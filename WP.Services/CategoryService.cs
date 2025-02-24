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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository repository)
        {
            _categoryRepository = repository;
        }

        public async Task<CategoryRequestDto> AddCategoryAsync(CategoryRequestDto category)
        {
            var createTerm = await _categoryRepository.AddCategoryAsync(category);
            return createTerm;
        }

        public async Task DeleteCategoryAsync(List<ulong> Ids)
        {
            await _categoryRepository.DeleteCategoryAsync(Ids);
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoryAsync()
        {
            return await _categoryRepository.GetAllCategoryAsync();
        }

        public async Task QuickUpdateCategoryAsync(WpTerm category)
        {
            await _categoryRepository.QuickUpdateCategoryAsync(category);
        }

        public async Task<bool> UpdateCategoryAsync(CategoryDto category)
        {
           return await _categoryRepository.UpdateCategoryAsync(category);
        }
    }
}
