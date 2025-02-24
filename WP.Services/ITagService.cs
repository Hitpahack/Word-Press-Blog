using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.Data;
using WP.DTOs;

namespace WP.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagResponseDto>> GetAllTagAsync();
        Task<TagRequestDto> AddTagAsync(TagRequestDto tag);
        Task DeleteTagAsync(List<ulong> Ids);
        Task QuickUpdateTagAsync(WpTerm tag);
        Task<bool> UpdateTagAsync(TagDto tag);

    }
}
