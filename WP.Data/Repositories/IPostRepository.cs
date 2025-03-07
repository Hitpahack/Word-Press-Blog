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

namespace WP.Data.Repositories
{
    public interface IPostRepository
    {
        Task<WpPost> GetPostByIdAsync(ulong Id);
        Task<WpPost> GetPostByNameAsync(string postName);
        Task<DataTableResponse<PostDto>> GetAllPostAsync(SearchModel filter);
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
                PostName = slug,
                PostDate = DateTime.Now,
                PostDateGmt = DateTime.UtcNow,
                PostType = "post"
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

        public async Task<DataTableResponse<PostDto>> GetAllPostAsync(SearchModel filter)
        {
            try
            {
                var wppost_predicate = PredicateBuilder.True<PostSearchDto>()
                    .And(s=>s.Post.PostStatus == "publish")
                    .And(s => s.Post.PostType == "post");


                if (!string.IsNullOrEmpty(filter.Status))
                    wppost_predicate = wppost_predicate.And(s => s.Post.PostStatus == filter.Status);

                if (!string.IsNullOrEmpty(filter.Date))
                    wppost_predicate = wppost_predicate.And(s => s.Post.PostDateGmt.Year == filter.DateTime.Year)
                        .And(s => s.Post.PostDateGmt.Month == filter.DateTime.Month);

                if (filter.CategoryId > 0)
                    wppost_predicate = wppost_predicate.And(s => s.WpTerm.TermId == filter.CategoryId);

                if (!string.IsNullOrEmpty(filter.Search?.Value) )
                {
                    string sva = filter.Search?.Value.ToLower();
                    wppost_predicate = wppost_predicate
                        .And(s => s.Post.PostTitle.ToLower().Contains(sva) || s.Post.PostName.ToLower().Contains(sva))
                        .Or(s => s.WpUser.UserNicename.ToLower().Contains(sva) || s.WpUser.UserLogin.ToLower().Contains(sva))
                         .Or(s =>s.WpTerm != null && s.WpTerm.Name.ToLower().Contains(sva));
                        
                }
                var fquery = _dbContext.WpPosts;

                int totalRecords = fquery.Count();
                
                var query = from post in fquery

               join user in _dbContext.WpUsers
                    on post.PostAuthor equals user.Id into userGroup
                from user in userGroup.DefaultIfEmpty() // Left Join
                
                join rel in _dbContext.WpTermRelationships
                    on post.Id equals rel.ObjectId into relGroup
                from rel in relGroup.DefaultIfEmpty() // Left Join

                join ttaxonomy in _dbContext.WpTermTaxonomies
                .Where(s => s.Taxonomy == "post_tag" || s.Taxonomy == "category")
                    on rel.TermTaxonomyId equals ttaxonomy.TermTaxonomyId into ttaxonmyGroup
                from ttaxonomy in ttaxonmyGroup.DefaultIfEmpty() // Left Join

                join term in _dbContext.WpTerms
                    on ttaxonomy.TermId equals term.TermId into termGroup
                from term in termGroup.DefaultIfEmpty() // Left Join

                join meta in _dbContext.WpPostmeta.Where(s=>s.MetaKey == "_thumbnail_id")
                    on post.Id equals meta.PostId into metaGroup
                from meta in metaGroup.DefaultIfEmpty() // Left Join

                //            join featImg in _dbContext.WpPosts
                //on EF.Functions.Collate(meta.MetaValue, "utf8mb4_unicode_ci") equals featImg.Id.ToString() into featImgGroup
                //            from featImg in featImgGroup.DefaultIfEmpty() // Left Join


                select new PostSearchDto
                {
                    Post = post,
                    WpUser = user,
                    PostRel = rel,
                    TermTaxonomy = ttaxonomy,
                    WpTerm = term,
                    Postmetum = meta,
                    //FeatiredImage = featImg
                };
                


                var filteredQuery = query
                 .Where(wppost_predicate.Compile())
                 .GroupBy(s => new { s.Post.Id }) // Grouping by Post ID to avoid duplication
                 .Select(s => new PostDto
                 {
                     Id = s.First().Post.Id,
                     Title = s.First().Post.PostTitle,
                     Author = s.First().WpUser.UserLogin,
                     PublishedDate = s.First().Post.PostModifiedGmt?.ToLocalTime(),
                     Categories = s.Where(x => x.TermTaxonomy?.Taxonomy == "category")
                                   .Select(x => x.WpTerm.Name)
                                   .Distinct(),
                     Tags = s.Where(x => x.TermTaxonomy?.Taxonomy == "post_tag")
                             .Select(x => x.WpTerm.Name)
                             .Distinct(),
                     Status = s.First().Post.PostStatus,
                     Content = s.First().Post.PostContent,
                     Excerpt = s.First().Post.PostExcerpt,
                     //FeaturedImage = s.First()?.FeatiredImage?.Guid
                 });

                int filteredRecords = filteredQuery.Count();

                // Apply pagination
                var skip = (filter.Page - 1) * (filter.PageSize ?? 10);
                var data = filteredQuery.Skip(skip??0).Take(filter.PageSize ?? 10).ToList();

                // Return correct response
                return new DataTableResponse<PostDto>(data, filter.Draw, totalRecords, filteredRecords);

            }
            catch (Exception ex)
            {
            }
            return new DataTableResponse<PostDto>(null, filter.Draw, 0, 0);
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
            //_dbContext.WpPosts.Update(post);
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
