using AutoMapper;
using WP.Data;
using WP.Data.Repositories;
using WP.DTOs;

namespace WP.Services
{
    public interface IPostService   
    {
        Task<ApiResponse<DataTableResponse<PostDto>>> GetAllPostsAsync(SearchModel filter);
        Task<ApiResponse<ulong>> CreatePostAsync(CreatePostDto post, ulong postid = 0);
        Task<ApiResponse<int>> DeletePostAsync(List<ulong> Ids);
        Task<ApiResponse<ulong>> UpdatePostAsync(ulong Id, CreatePostDto post);
        Task<WpPost> GetPostByNameAsync(string postTitle);
        Task<PostDto> GetPost(ulong id);
    }
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        public PostService(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ulong>> CreatePostAsync(CreatePostDto postDto, ulong postid = 0)
        {
            ulong result = await _postRepository.CreatePostAsync(postDto);
            if (result == 0)
            {
                return new FailedApiResponse<ulong>( "Failed to create post.");
            }
            return new SuccessApiResponse<ulong>(result, "Post created successfully.");
        }

        public async Task<ApiResponse<DataTableResponse<PostDto>>> GetAllPostsAsync(SearchModel filter)
        {
            var posts = await _postRepository.GetAllPostAsync(filter);
            return new SuccessApiResponse<DataTableResponse<PostDto>>(posts, "Posts retrieved successfully.");
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

        public async Task<ApiResponse<ulong>> UpdatePostAsync(ulong Id, CreatePostDto post)
        {
            WpPost existingPost = await _postRepository.GetPostByIdAsync(Id);
            if (existingPost == null)
            {
                return new FailedApiResponse<ulong>( "Post not found.");
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
            return new SuccessApiResponse<ulong>(existingPost.Id, "Post updated successfully.");
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

        public async Task<PostDto> GetPost(ulong id)
        {
            WpPost post = await _postRepository.GetPostByIdAsync(id);
            if (post != null)
            {
                return _mapper.Map<PostDto>(post);
            }
            return null;
        }  

    }

}
