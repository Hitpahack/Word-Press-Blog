using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.DTOs;

namespace WP.Data.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<TagResponseDto>> GetAllTagAsync();
        Task<TagRequestDto> AddTagAsync(TagRequestDto tag);
        Task<bool> DeleteTagAsync(List<ulong> Ids);
        Task<WpTerm> QuickUpdateTagAsync(WpTerm tag);
        Task<WpTerm> UpdateTagAsync(UpdateTagDto tag);
    }
    public class TagRepository : ITagRepository     
    {
        private readonly BlogContext _dbContext;

        public TagRepository(BlogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TagRequestDto> AddTagAsync(TagRequestDto tag)
        {
            var term = new WpTerm
            {
                Name = tag.Name,
                Slug = tag.Slug,
            };
            _dbContext.WpTerms.Add(term);
            await _dbContext.SaveChangesAsync();

            var termTaxonomy = new WpTermTaxonomy
            {
                Description = tag.Description,
                Taxonomy = "post_tag",
                TermId = term.TermId,
            };

            _dbContext.WpTermTaxonomies.Add(termTaxonomy);
            await _dbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<bool> DeleteTagAsync(List<ulong> Ids)
        {
            var tagToDelete = await _dbContext.WpTerms.Where(term => Ids.Contains(term.TermId)).ToListAsync();
            if (tagToDelete.Any())
            {
                _dbContext.WpTerms.RemoveRange(tagToDelete);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<TagResponseDto>> GetAllTagAsync()
        {
            var terms = await (from term in _dbContext.WpTerms
                               join taxonomy in _dbContext.WpTermTaxonomies
                               on term.TermId equals taxonomy.TermId
                               where taxonomy.Taxonomy == "post_tag"
                               select new TagResponseDto
                               {
                                   Name = term.Name,
                                   Description = taxonomy.Description,
                                   Count = taxonomy.Count,
                                   Slug = term.Slug,
                                   TermId = term.TermId,
                                   TermTaxonomyId = taxonomy.TermTaxonomyId,
                               }).ToListAsync();
            return terms;
        }

        public async Task<WpTerm> QuickUpdateTagAsync(WpTerm tag)
        {
            var currrentTag= await _dbContext.WpTerms.FirstOrDefaultAsync(t => t.TermId == tag.TermId);
            if (currrentTag == null)
            {
                throw new KeyNotFoundException("Tag not found.");
            }
            currrentTag.Name = tag.Name;
            currrentTag.Slug = tag.Slug;
            _dbContext.WpTerms.Update(currrentTag);
            await _dbContext.SaveChangesAsync();
            return currrentTag;
        }

        public async Task<WpTerm> UpdateTagAsync(UpdateTagDto tag)
        {
            var term = await _dbContext.WpTerms.FindAsync(tag.Id);
            var taxonomy = await _dbContext.WpTermTaxonomies.FirstOrDefaultAsync(t => t.TermId == tag.Id);
            if (term == null || taxonomy == null)
            {
                throw new KeyNotFoundException("Tag not found.");
            }
            term.Name = tag.Name;
            term.Slug = tag.Slug;

            taxonomy.Description = tag.Description;

            _dbContext.WpTerms.Update(term);
            _dbContext.WpTermTaxonomies.Update(taxonomy);
            await _dbContext.SaveChangesAsync();
            return term;
        }
    }

}
