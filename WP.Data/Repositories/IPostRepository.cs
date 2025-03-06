using Abp;
using Abp.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WP.Data.Repositories
{
    public interface IPostRepository
    {
        Task<WpPost> GetPostByIdAsync(ulong Id);
        Task<WpPost> GetPostByNameAsync(string postName);
        Task<IEnumerable<PostDto>> GetAllPostAsync(SearchModel filter);
        Task<ulong> CreatePostAsync(CreatePostDto postDto);
        Task<int> DeletePostAsync(List<ulong> Id);
        Task<bool> UpdatePostAsync(WpPost post);
        Task DeletePostCategoriesAndTagsAsync(ulong postId);
        Task AddCategoriesAndTagsAsync(ulong postId, List<ulong> categories, List<ulong> tags);
    }
    public class PostRepository : IPostRepository
    {
        private readonly BlogContext _dbContext;
        private readonly ILogger<PostRepository> _logger;

        public PostRepository(BlogContext dbContext, ILogger<PostRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ulong> CreatePostAsync(CreatePostDto postDto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {


                string slug = postDto.Title.ToLower().Replace(" ", "-");

                var post = new WpPost
                {
                    PostAuthor = postDto.AuthorId,
                    PostTitle = postDto.Title,
                    PostContent = postDto.Content,
                    PostExcerpt = postDto.Excerpt,
                    PostStatus = postDto.Status,
                    PostName = slug
                };

                _dbContext.WpPosts.AddAsync(post);
                await _dbContext.SaveChangesAsync();

                ulong postId = post.Id;

                // Insert categories into `wp_term_relationships`
                foreach (var categoryId in postDto.Categories)
                {
                    await _dbContext.WpTermRelationships.AddAsync(new WpTermRelationship
                    {
                        ObjectId = postId,
                        TermTaxonomyId = categoryId
                    });
                }

                // Insert tags into `wp_term_relationships`
                foreach (var tagId in postDto.Tags)
                {
                    await _dbContext.WpTermRelationships.AddAsync(new WpTermRelationship
                    {
                        ObjectId = postId,
                        TermTaxonomyId = tagId
                    });
                }

                // Insert Featured Image (if provided)
                if (!string.IsNullOrEmpty(postDto.FeaturedImageUrl))
                {
                    await _dbContext.WpPostmeta.AddAsync(new WpPostmetum
                    {
                        PostId = postId,
                        MetaKey = "_thumbnail_id",
                        MetaValue = postDto.FeaturedImageUrl
                    });
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return postId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating post with title: {Title}", postDto.Title);
                return 0;
            }
        }

        public async Task<IEnumerable<PostDto>> GetAllPostAsync(SearchModel filter)
        {
            try
            {
                var wppost_predicate = PredicateBuilder.True<PostSearchDto>().And(p => p.Post.PostType == "post");


                if (!string.IsNullOrEmpty(filter.Status))
                    wppost_predicate = wppost_predicate.And(s => s.Post.PostStatus == filter.Status);

                if (!string.IsNullOrEmpty(filter.Date))
                    wppost_predicate = wppost_predicate.And(s => s.Post.PostDateGmt.Year == filter.DateTime.Year)
                        .And(s => s.Post.PostDateGmt.Month == filter.DateTime.Month);

                if (filter.CategoryId > 0)
                    wppost_predicate = wppost_predicate.And(s => s.WpTerm.TermId == filter.CategoryId);

                var query = from post in _dbContext.WpPosts

                            join rel in _dbContext.WpTermRelationships
                                on post.Id equals rel.ObjectId into relGroup
                            from rel in relGroup.DefaultIfEmpty() // Left Join

                            join user in _dbContext.WpUsers
                                on post.PostAuthor equals user.Id into userGroup
                            from user in userGroup.DefaultIfEmpty() // Left Join

                            join term in _dbContext.WpTerms
                                on rel.TermTaxonomyId equals term.TermId into termGroup
                            from term in termGroup.DefaultIfEmpty() // Left Join

                            join meta in _dbContext.WpPostmeta
                                .Where(s => s.MetaKey == "_thumbnail_id")
                                on post.Id equals meta.PostId into metaGroup
                            from meta in metaGroup.DefaultIfEmpty() // Left Join

                            select new PostSearchDto
                            {
                                Post = post,
                                PostRel = rel,
                                WpUser = user,
                                WpTerm = term,
                                Postmetum = meta
                            };
                var filteredQuery = query.Where(wppost_predicate.Compile());
                var result = filteredQuery.ToList();
            }
            catch (Exception ex)
            {

                
            }




            //var posts = await query
            //  .OrderByDescending(p => p.Post)
            //  .Skip((filter.Page - 1) * filter.PageSize)
            //  .Take(filter.PageSize)
            //  .Select(p => new PostDto
            //  {
            //      Id = p.Post.Id,
            //      Title = p.Post.PostTitle,
            //      Content = p.Post.PostContent,
            //      Excerpt = p.Post.PostExcerpt,
            //      Status = p.Post.PostStatus,
            //      PublishedDate = p.Post.PostDate,
            //      Author = p.WpUser.UserLogin,
            //      Categories = p.re _dbContext.WpTermRelationships
            //          .Where(r => r.ObjectId == p.Id)
            //          .Select(r => _dbContext.WpTerms
            //              .Where(t => t.TermId == r.TermTaxonomyId)
            //              .Select(t => t.Name)
            //              .FirstOrDefault()
            //          ).ToList(),
            //      Tags = _dbContext.WpTermRelationships
            //          .Where(r => r.ObjectId == p.Id)
            //          .Select(r => _dbContext.WpTerms
            //              .Where(t => t.TermId == r.TermTaxonomyId)
            //              .Select(t => t.Name)
            //              .FirstOrDefault()
            //          ).ToList(),
            //      FeaturedImage = _dbContext.WpPostmeta
            //          .Where(m => m.PostId == p.Id && m.MetaKey == "_thumbnail_id")
            //          .Select(m => m.MetaValue)
            //          .FirstOrDefault()
            //  })
            //      .ToListAsync();

            return new List<PostDto>();
        }

        public async Task<int> DeletePostAsync(List<ulong> Ids)
        {
            var postToDelete = await _dbContext.WpPosts.Where(post => Ids.Contains(post.Id)).ToListAsync();
            if (postToDelete.Any())
            {
                foreach (var post in postToDelete)
                {
                    post.PostStatus = "trash";
                }
                await _dbContext.SaveChangesAsync();
                return postToDelete.Count();
            }
            return 0;
        }

        public async Task<bool> UpdatePostAsync(WpPost post)
        {
            _dbContext.WpPosts.Update(post);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<WpPost> GetPostByIdAsync(ulong Id)
        {
            return await _dbContext.WpPosts.FirstOrDefaultAsync(post => post.Id == Id);
        }

        public async Task<WpPost> GetPostByNameAsync(string postName)
        {
            return await _dbContext.WpPosts.FirstOrDefaultAsync(post => post.PostTitle == postName);
        }
        public async Task DeletePostCategoriesAndTagsAsync(ulong postId)
        {
            await _dbContext.WpTermRelationships
                .Where(r => r.ObjectId == postId)
                .ExecuteDeleteAsync();
        }
        public async Task AddCategoriesAndTagsAsync(ulong postId, List<ulong> categories, List<ulong> tags)
        {
            var relationships = new List<WpTermRelationship>();

            if (categories != null)
            {
                relationships.AddRange(categories.Select(categoryId => new WpTermRelationship
                {
                    ObjectId = postId,
                    TermTaxonomyId = categoryId
                }));
            }

            if (tags != null)
            {
                relationships.AddRange(tags.Select(tagId => new WpTermRelationship
                {
                    ObjectId = postId,
                    TermTaxonomyId = tagId
                }));
            }

            await _dbContext.WpTermRelationships.AddRangeAsync(relationships);
            await _dbContext.SaveChangesAsync();
        }

    }

}
