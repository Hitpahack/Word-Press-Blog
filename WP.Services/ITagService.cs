using Abp.Extensions;
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
    public interface ITagService
    {
        Task<ApiResponse<IEnumerable<TagResponseDto>>> GetAllTagAsync();
        Task<ApiResponse<TagRequestDto>> AddTagAsync(TagRequestDto tag);
        Task<ApiResponse<bool>> DeleteTagAsync(List<ulong> Ids);
        Task<ApiResponse<WpTerm>> QuickUpdateTagAsync(WpTerm tag);
        Task<ApiResponse< WpTerm>> UpdateTagAsync(UpdateTagDto tag);

    }
    public class TagService : ITagService
    {
        public readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<ApiResponse<TagRequestDto>> AddTagAsync(TagRequestDto tag)
        {
            TagRequestDto createTerm = await _tagRepository.AddTagAsync(tag);
            if (createTerm == null)
                return new FailedApiResponse<TagRequestDto>("Failed to add tag");
            return new SuccessApiResponse<TagRequestDto>(createTerm,"Tag added sucessfully");
        }

        public async Task<ApiResponse<bool>> DeleteTagAsync(List<ulong> Ids)
        {
            bool result = await _tagRepository.DeleteTagAsync(Ids);
            if(!result)
                return new FailedApiResponse<bool>("Failed to delete tag");
            return new SuccessApiResponse<bool>(true, "Tag deleted sucessfully");
        }

        public async Task<ApiResponse<IEnumerable<TagResponseDto>>> GetAllTagAsync()
        {
            var result =  await _tagRepository.GetAllTagAsync();
            if(result == null || result.Any())
                return new FailedApiResponse<IEnumerable<TagResponseDto>>("Failed to get tags");
            return new SuccessApiResponse<IEnumerable<TagResponseDto>>(result, "Tag deleted sucessfully");
        }

        public async Task<ApiResponse<WpTerm>> QuickUpdateTagAsync(WpTerm tag)
        {
            WpTerm result = await _tagRepository.QuickUpdateTagAsync(tag);
            if(result==null)
                return new FailedApiResponse<WpTerm>("Failed to quick update tags");
            return new SuccessApiResponse<WpTerm>(result, "Tag updated sucessfully");
        }

        public async Task<ApiResponse<WpTerm>> UpdateTagAsync(UpdateTagDto tag)
        {
            WpTerm result = await _tagRepository.UpdateTagAsync(tag);
            if (result == null)
                return new FailedApiResponse<WpTerm>("Failed to update tags");
            return new SuccessApiResponse<WpTerm>(result, "Tag updated sucessfully");

        }
    }

}
