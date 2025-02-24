using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.Data;
using WP.DTOs;

namespace WP.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoryAsync();
        Task<CategoryRequestDto> AddCategoryAsync(CategoryRequestDto category);
        Task DeleteCategoryAsync(List<ulong> Ids);
        Task QuickUpdateCategoryAsync(WpTerm category);
        Task<bool> UpdateCategoryAsync(CategoryDto category);

    }
}
        