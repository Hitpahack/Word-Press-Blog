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
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPostsAsync();
        Task<bool> CreatePostAsync(WpPost post);
        Task DeletePostAsync(List<ulong> Ids);  
        Task UpdatePostAsync(WpPost post);
        Task<WpPost> GetPostByNameAsync(string postTitle);
    }
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<bool> CreatePostAsync(WpPost post)
        {
            var newPost = new WpPost
            {
                Id = post.Id,
                CommentCount = post.CommentCount,
                CommentStatus = post.CommentStatus,
                Guid = post.Guid,
                MenuOrder = post.MenuOrder,
                Pinged = post.Pinged,
                PingStatus = post.PingStatus,
                PostAuthor = post.PostAuthor,
                PostContent = post.PostContent,
                PostContentFiltered = post.PostContentFiltered,
                PostDate = post.PostDate,
                PostDateGmt = post.PostDateGmt,
                PostExcerpt = post.PostExcerpt,
                PostMimeType = post.PostMimeType,
                PostModified = post.PostModified,
                PostTitle = post.PostTitle,
                PostModifiedGmt = post.PostModifiedGmt,
                PostName = post.PostName,
                PostParent = post.PostParent,
                PostStatus = post.PostStatus,
                PostPassword = post.PostPassword,
                PostType = "post",
                ToPing = post.ToPing,
            };

            await _postRepository.CreatePostAsync(newPost);
            return true;
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllPostAsync();
            return posts;
        }

        public async Task DeletePostAsync(List<ulong> Ids)
        {
            await _postRepository.DeletePostAsync(Ids);
        }

        public async Task UpdatePostAsync(WpPost post)
        {
            var existingPost = await _postRepository.GetPostByIdAsync(post.Id);
            if (existingPost == null)
            {
                throw new KeyNotFoundException("Post not found.");
            }
            existingPost.CommentCount = post.CommentCount;
            existingPost.CommentStatus = post.CommentStatus;
            existingPost.Guid = post.Guid;
            existingPost.MenuOrder = post.MenuOrder;
            existingPost.Pinged = post.Pinged;
            existingPost.PingStatus = post.PingStatus;
            existingPost.PostAuthor = post.PostAuthor;
            existingPost.PostContent = post.PostContent;
            existingPost.PostContentFiltered = post.PostContentFiltered;
            existingPost.PostDate = post.PostDate;
            existingPost.PostDateGmt = post.PostDateGmt;
            existingPost.PostExcerpt = post.PostExcerpt;
            existingPost.PostMimeType = post.PostMimeType;
            existingPost.PostModified = post.PostModified;
            existingPost.PostTitle = post.PostTitle;
            existingPost.PostModifiedGmt = post.PostModifiedGmt;
            existingPost.PostName = post.PostName;
            existingPost.PostParent = post.PostParent;
            existingPost.PostStatus = post.PostStatus;
            existingPost.PostPassword = post.PostPassword;
            existingPost.PostType = post.PostType;
            existingPost.ToPing = post.ToPing;

            await _postRepository.UpdatePostAsync(existingPost);

        }

        public async Task<WpPost> GetPostByNameAsync(string postTitle)
        {
            var post = await _postRepository.GetPostByNameAsync(postTitle);
            if (post != null)
            {
                return post;
            }
            return null;
        }
    }

}
