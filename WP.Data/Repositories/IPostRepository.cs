using Microsoft.EntityFrameworkCore;
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
        Task<IEnumerable<PostDto>> GetAllPostAsync(string status, int page, int pageSize );
        Task<ulong> CreatePostAsync(CreatePostDto postDto);
        Task<int> DeletePostAsync(List<ulong> Id);
        Task<bool> UpdatePostAsync(WpPost post);
        Task DeletePostCategoriesAndTagsAsync(ulong postId);
        Task AddCategoriesAndTagsAsync(ulong postId, List<ulong> categories, List<ulong> tags);
    }
    public class PostRepository : IPostRepository
    {
        private readonly BlogContext _dbContext;

        public PostRepository(BlogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ulong> CreatePostAsync(CreatePostDto postDto)
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
            return postId;

        }

        public async Task<IEnumerable<PostDto>> GetAllPostAsync(string status = "publish", int page = 1, int pageSize = 10)
        {
            var posts = await _dbContext.WpPosts
             .Where(p => p.PostType == "post" && p.PostStatus == status)
             .OrderByDescending(p => p.PostDate)
            .Skip((page - 1) * pageSize)
             .Take(pageSize)
             .Select(p => new PostDto
             {
                 Id = p.Id,
                 Title = p.PostTitle,
                 Content = p.PostContent,
                 Excerpt = p.PostExcerpt,
                 Status = p.PostStatus,
                 PublishedDate = p.PostDate,
                 Author = _dbContext.WpUsers.Where(u => u.Id == p.PostAuthor).Select(u => u.UserLogin).FirstOrDefault(),
                 Categories = _dbContext.WpTermRelationships
                     .Where(r => r.ObjectId == p.Id)
                     .Select(r => _dbContext.WpTerms
                         .Where(t => t.TermId == r.TermTaxonomyId)
                         .Select(t => t.Name)
                         .FirstOrDefault()
                     ).ToList(),
                 Tags = _dbContext.WpTermRelationships
                     .Where(r => r.ObjectId == p.Id)
                     .Select(r => _dbContext.WpTerms
                         .Where(t => t.TermId == r.TermTaxonomyId)
                         .Select(t => t.Name)
                         .FirstOrDefault()
                     ).ToList(),
                 FeaturedImage = _dbContext.WpPostmeta
                     .Where(m => m.PostId == p.Id && m.MetaKey == "_thumbnail_id")
                     .Select(m => m.MetaValue)
                     .FirstOrDefault()
             })
             .ToListAsync();

            return posts;
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
