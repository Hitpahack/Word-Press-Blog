using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        Task<bool> QuickUpdateCategoryAsync(WpTerm category);
        Task<bool> DeleteCategoryAsync(List<ulong> Ids);
        Task<WpTerm> GetCategoryByIdAsync(ulong id);
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoryAsync();


    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlogContext _dbContext;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(BlogContext dbContext, ILogger<CategoryRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<CategoryRequestDto> AddCategoryAsync(CategoryRequestDto category)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {

                var term = new WpTerm
                {
                    Name = category.Name,
                    Slug = category.Slug,
                };
                _dbContext.WpTerms.Add(term);
                await _dbContext.SaveChangesAsync();

                var termTaxonomy = new WpTermTaxonomy
                {
                    Description = category.Description,
                    Parent = category.Parent,
                    Taxonomy = "category",
                    TermId = term.TermId,
                };

                _dbContext.WpTermTaxonomies.Add(termTaxonomy);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return category;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error adding category: {CategoryName}", category.Name);
                return null;
            }
        }

        public async Task DeleteCategoryAsync(List<ulong> Ids)
        {
            var categoryToDelete = await _dbContext.WpTerms.Where(term => Ids.Contains(term.TermId)).ToListAsync();
            if (categoryToDelete.Any())
            {
                _dbContext.WpTerms.RemoveRange(categoryToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoryAsync()
        {
            var terms = await (from term in _dbContext.WpTerms
                               join taxonomy in _dbContext.WpTermTaxonomies
                               on term.TermId equals taxonomy.TermId
                               where taxonomy.Taxonomy == "category"
                               select new CategoryResponseDto
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

        public async Task<WpTerm> GetCategoryByIdAsync(ulong id)
        {
            return await _dbContext.WpTerms.FirstOrDefaultAsync(term => term.TermId == id);
        }

        public async Task<bool> QuickUpdateCategoryAsync(WpTerm category)
        {
            var currrentCategory = await _dbContext.WpTerms.FirstOrDefaultAsync(t => t.TermId == category.TermId);
            if (currrentCategory == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            currrentCategory.Name = category.Name;
            currrentCategory.Slug = category.Slug;
            _dbContext.WpTerms.Update(currrentCategory);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryDto category)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var term = await _dbContext.WpTerms.FindAsync(category.Id);
                var taxonomy = await _dbContext.WpTermTaxonomies.FirstOrDefaultAsync(t => t.TermId == category.Id);
                if (term == null || taxonomy == null)
                {
                    return false;
                }
                term.Name = category.Name;
                term.Slug = category.Slug;

                taxonomy.Parent = category.Parent;
                taxonomy.Description = category.Description;

                _dbContext.WpTerms.Update(term);
                _dbContext.WpTermTaxonomies.Update(taxonomy);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error updating category ID: {CategoryId}", category.Id);
                return false;
            }
        }

        async Task<bool> ICategoryRepository.DeleteCategoryAsync(List<ulong> Ids)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var termToDelete = await _dbContext.WpTerms.Where(term => Ids.Contains(term.TermId)).ToListAsync();
                var taxonoyToDelete = await _dbContext.WpTermTaxonomies.Where(taxo => Ids.Contains(taxo.TermId)).ToListAsync();
                if (termToDelete.Any() || taxonoyToDelete.Any())
                {
                    _dbContext.WpTerms.RemoveRange(termToDelete);
                    _dbContext.WpTermTaxonomies.RemoveRange(taxonoyToDelete);
                    await _dbContext.SaveChangesAsync();
                }
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error deleting categories with IDs: {CategoryIds}", string.Join(", ", Ids));
                return false;
            }
        }
    }

}
