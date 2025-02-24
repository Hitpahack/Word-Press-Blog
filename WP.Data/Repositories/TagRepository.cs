using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.DTOs;

namespace WP.Data.Repositories
{
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

        public async Task DeleteTagAsync(List<ulong> Ids)
        {
            var tagToDelete = await _dbContext.WpTerms.Where(term => Ids.Contains(term.TermId)).ToListAsync();
            if (tagToDelete.Any())
            {
                _dbContext.WpTerms.RemoveRange(tagToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TagResponseDto>> GetAllTagAsync()
        {
            var terms = await(from term in _dbContext.WpTerms
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

        public async Task QuickUpdateTagAsync(WpTerm tag)
        {
            var currrentCategory = await _dbContext.WpTerms.FirstOrDefaultAsync(t => t.TermId == tag.TermId);
            if (currrentCategory == null)
            {
                throw new KeyNotFoundException("Tag not found.");
            }
            currrentCategory.Name = tag.Name;
            currrentCategory.Slug = tag.Slug;
            _dbContext.WpTerms.Update(currrentCategory);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateTagAsync(TagDto tag)
        {
            var term = await _dbContext.WpTerms.FindAsync(tag.Id);
            var taxonomy = await _dbContext.WpTermTaxonomies.FirstOrDefaultAsync(t => t.TermId == tag.Id);
            if (term == null || taxonomy == null)
            {
                return false;
            }
            term.Name = tag.Name;
            term.Slug = tag.Slug;

            taxonomy.Description = tag.Description;

            _dbContext.WpTerms.Update(term);
            _dbContext.WpTermTaxonomies.Update(taxonomy);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
