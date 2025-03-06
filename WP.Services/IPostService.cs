using Microsoft.EntityFrameworkCore;
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
        Task<ApiResponse<IEnumerable<PostDto>>> GetAllPostsAsync(SearchModel filter);
        Task<ApiResponse<ulong>> CreatePostAsync(CreatePostDto post);
        Task<ApiResponse<int>> DeletePostAsync(List<ulong> Ids);
        Task<ApiResponse<string>> UpdatePostAsync(ulong Id, UpdatePostDto post);
        Task<WpPost> GetPostByNameAsync(string postTitle);
    }
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<ApiResponse<ulong>> CreatePostAsync(CreatePostDto postDto)
        {
            ulong result = await _postRepository.CreatePostAsync(postDto);
            if (result == 0)
            {
                return new FailedApiResponse<ulong>( "Failed to create post.");
            }
            return new SuccessApiResponse<ulong>(result, "Post created successfully.");
        }

        public async Task<ApiResponse<IEnumerable<PostDto>>> GetAllPostsAsync(SearchModel filter)
        {
            var posts = await _postRepository.GetAllPostAsync(filter);
            if (posts == null || !posts.Any())
                return new FailedApiResponse<IEnumerable<PostDto>>( "No posts found.");
            return new SuccessApiResponse<IEnumerable<PostDto>>(posts, "Posts retrieved successfully.");
        }

        public async Task<ApiResponse<int>> DeletePostAsync(List<ulong> Ids)
        {
            int resultCount = await _postRepository.DeletePostAsync(Ids);
            if (resultCount == 0)
            {
                return new FailedApiResponse<int>( "No matching posts found.");
            }
            return new SuccessApiResponse<int>(resultCount, $"Successfully deleted {resultCount} posts.");
        }

        public async Task<ApiResponse<string>> UpdatePostAsync(ulong Id, UpdatePostDto post)
        {
            WpPost existingPost = await _postRepository.GetPostByIdAsync(Id);
            if (existingPost == null)
            {
                return new FailedApiResponse<string>( "Post not found.");
            }
            existingPost.PostTitle = post.Title;
            existingPost.PostContent = post.Content;
            existingPost.PostExcerpt = post.Excerpt;
            existingPost.PostStatus = post.Status;
            existingPost.PostDate = DateTime.Now;

            await _postRepository.UpdatePostAsync(existingPost);
            if ((post.Categories != null && post.Categories.Any()) ||   (post.Tags != null && post.Tags.Any()))
            {
                await _postRepository.DeletePostCategoriesAndTagsAsync(Id);
                await _postRepository.AddCategoriesAndTagsAsync(Id, post.Categories, post.Tags);
            }
            return new SuccessApiResponse<string>("Post updated successfully.", "Post updated successfully.");
        }

        public async Task<WpPost> GetPostByNameAsync(string postTitle)
        {
            WpPost post = await _postRepository.GetPostByNameAsync(postTitle);
            if (post != null)
            {
                return post;
            }
            return null;
        }
    }

}
