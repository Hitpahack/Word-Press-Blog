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
        Task<ApiResponse<IEnumerable<PostDto>>> GetAllPostsAsync(string status, int page, int pageSize);
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
            var result = await _postRepository.CreatePostAsync(postDto);
            if (result == 0)
            {
                return new ApiResponse<ulong>(false, "Failed to create post.", 0, 500);
            }
            return new ApiResponse<ulong>(true, "Post created successfully.", result, 201);
        }

        public async Task<ApiResponse<IEnumerable<PostDto>>> GetAllPostsAsync(string status, int page = 1, int pageSize = 10)
        {
            var posts = await _postRepository.GetAllPostAsync(status, page, pageSize);
            if (posts == null || !posts.Any())
                return new ApiResponse<IEnumerable<PostDto>>(false, "No posts found.", null, 404);
            return new ApiResponse<IEnumerable<PostDto>>(true, "Posts retrieved successfully.", posts, 200);
        }

        public async Task<ApiResponse<int>> DeletePostAsync(List<ulong> Ids)
        {
            int resultCount = await _postRepository.DeletePostAsync(Ids);
            if (resultCount == 0)
            {
                return new ApiResponse<int>(false, "No matching posts found.", 0, 404);
            }
            return new ApiResponse<int>(true, $"Successfully deleted {resultCount} posts.", resultCount, 200);
        }

        public async Task<ApiResponse<string>> UpdatePostAsync(ulong Id, UpdatePostDto post)
        {
            var existingPost = await _postRepository.GetPostByIdAsync(Id);
            if (existingPost == null)
            {
                return new ApiResponse<string>(false, "Post not found.", null, 404);
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
            return new ApiResponse<string>(true, "Post updated successfully.", null, 200);
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
