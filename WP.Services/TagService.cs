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
    public class TagService : ITagService
    {
        public readonly ITagRepository _tagRepository ;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<TagRequestDto> AddTagAsync(TagRequestDto tag)
        {
            var createTerm = await _tagRepository.AddTagAsync(tag);
            return createTerm;
        }

        public async Task DeleteTagAsync(List<ulong> Ids)
        {
            await _tagRepository.DeleteTagAsync(Ids);
        }

        public async Task<IEnumerable<TagResponseDto>> GetAllTagAsync()
        {
            return await _tagRepository.GetAllTagAsync();
        }

        public async Task QuickUpdateTagAsync(WpTerm tag)
        {
            await _tagRepository.QuickUpdateTagAsync(tag);
        }

        public async Task<bool> UpdateTagAsync(TagDto tag)
        {
            return await _tagRepository.UpdateTagAsync(tag);
        }
    }
}
