using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WP.Data;
using WP.Data.Repositories;
using WP.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WP.Services
{
    public interface ICommentService
    {
        Task<ApiResponse<IEnumerable<WpComment>>> GetAllCommentsAsync();
        Task<ApiResponse<WpComment>> GetCommentById(ulong id);
        Task<ApiResponse<WpComment>> AddComment(CreateCommentDto createDto);
        Task<ApiResponse<WpComment>> UpdateComment(ulong id, UpdateCommentDto updateDto);
        Task<ApiResponse<bool>> DeleteComment(List<ulong> id);
        Task<ApiResponse<bool>> ApproveCommentAsync(ulong commentId);
        Task<ApiResponse<bool>> DisapproveCommentAsync(ulong commentId);
        Task<ApiResponse<bool>> MarkAsSpamAsync(ulong commentId);
        Task<ApiResponse<bool>> UnspamCommentAsync(ulong commentId);
        Task<ApiResponse<WpComment>> ReplyToCommentAsync( ReplyRequest request);
    }
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<ApiResponse<IEnumerable<WpComment>>> GetAllCommentsAsync()
        {
            IEnumerable<WpComment> comments = await _commentRepository.GetAllCommentsAsync();
            if(comments==null || !comments.Any())
                return new FailedApiResponse<IEnumerable<WpComment>>("No comments found.");

            return new SuccessApiResponse<IEnumerable<WpComment>>(comments, "Comments retrieved successfully.");
        }

        public async Task<ApiResponse<WpComment>> GetCommentById(ulong id)
        {
            WpComment comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
                return new FailedApiResponse<WpComment>("No comment found.");

            return new SuccessApiResponse<WpComment>(comment, "Comment retrieved successfully.");
        }

        public async Task<ApiResponse<WpComment>> AddComment(CreateCommentDto createDto)
        {
            var comment = new WpComment
            {
                CommentPostId = createDto.CommentPostId,
                CommentAuthor = createDto.CommentAuthor,
                CommentAuthorEmail = createDto.CommentAuthorEmail,
                CommentContent = createDto.CommentContent,
                UserId = createDto.UserId,
                CommentDate = DateTime.UtcNow,
                CommentDateGmt = DateTime.UtcNow,
                CommentAuthorIp = createDto.CommentAuthorIp,
                CommentApproved = "pending",
                CommentAgent = createDto.CommentAgent,
                CommentType = "comment",
                CommentParent = 0,
            };
            WpComment newComment = await _commentRepository.AddCommentAsync(comment);
            if (newComment == null)
                return new FailedApiResponse<WpComment>("Failed to add comment.");

            return new SuccessApiResponse<WpComment>(newComment, "Comment added successfully.");
        }

        public async Task<ApiResponse<WpComment>> UpdateComment(ulong id, UpdateCommentDto updateDto)
        {
            WpComment updatedComment = await _commentRepository.UpdateCommentAsync(id, updateDto);
            if (updatedComment == null)
                return new FailedApiResponse<WpComment>("Failed to update comment.");

            return new SuccessApiResponse<WpComment>(updatedComment, "Comment updated successfully.");


        }

        public async Task<ApiResponse<bool>> DeleteComment(List<ulong> id)
        {
            bool result = await _commentRepository.DeleteCommentAsync(id);
            if(!result)
                return new FailedApiResponse<bool>("Failed to delete comment.");

            return new SuccessApiResponse<bool>(true, "Comment deleted successfully.");
        }

        public async Task<ApiResponse<bool>> ApproveCommentAsync(ulong commentId)
        {
            bool result  = await _commentRepository.UpdateCommentStatusAsync(commentId, "approved");
            if(!result)
                return new FailedApiResponse<bool>("Failed to approve comment.");

            return new SuccessApiResponse<bool>(true, "Comment approved successfully.");
        }

        public async Task<ApiResponse<bool>> DisapproveCommentAsync(ulong commentId)
        {
            bool result= await _commentRepository.UpdateCommentStatusAsync(commentId, "hold");
            if(!result)
                return new FailedApiResponse<bool>("Failed to unapprove comment.");

            return new SuccessApiResponse<bool>(true, "Comment unapproved successfully.");

        }

        public async Task<ApiResponse<bool>> MarkAsSpamAsync(ulong commentId)
        {
            bool result = await _commentRepository.UpdateCommentStatusAsync(commentId, "spam");
            if(!result)
                return new FailedApiResponse<bool>("Failed to spam comment.");

            return new SuccessApiResponse<bool>(true, "Comment marked as spam successfully.");

        }

        public async Task<ApiResponse<bool>> UnspamCommentAsync(ulong commentId)
        {
            bool result = await _commentRepository.UpdateCommentStatusAsync(commentId, "approved");
            if(!result)
                return new FailedApiResponse<bool>("Failed to unspam comment.");

            return new SuccessApiResponse<bool>(true, "Comment removed from spam successfully.");

        }

        public async Task<ApiResponse<WpComment>> ReplyToCommentAsync(ReplyRequest request)
        {
            WpComment replyComment = new WpComment
            {
                CommentPostId = request.PostId,
                CommentParent = request.ParentCommentId,
                CommentAuthor = request.Author,
                CommentAuthorEmail = request.AuthorEmail,
                CommentContent = request.Content,
                CommentApproved = "aprooved",
                CommentDate = DateTime.UtcNow,
                CommentDateGmt = DateTime.UtcNow,
            };
            WpComment newComment = await _commentRepository.AddCommentAsync(replyComment);
            if(newComment == null)
                return new FailedApiResponse<WpComment>("Failed to reply to the comment.");

            return new SuccessApiResponse<WpComment>(newComment, "Reply added successfully.");

        }
    }
}
