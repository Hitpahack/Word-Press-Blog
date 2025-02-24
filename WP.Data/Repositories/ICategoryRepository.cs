using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.DTOs;

namespace WP.Data.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryRequestDto> AddCategoryAsync(CategoryRequestDto category);
        Task<bool> UpdateCategoryAsync(CategoryDto category);
        Task QuickUpdateCategoryAsync(WpTerm category);
        Task<bool> DeleteCategoryAsync(List<ulong> Ids);
        Task<WpTerm> GetCategoryByIdAsync(ulong id);
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoryAsync();
       

    }
}
