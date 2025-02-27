using AutoMapper;
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
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<IEnumerable<WpComment>> GetAllCommentsAsync()
        {
            var comments = await _commentRepository.GetAllCommentsAsync();
            return comments;
        }

        public async Task<WpComment> GetCommentById(ulong id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            return comment;
        }

        public async Task<WpComment> AddComment(CreateCommentDto createDto)
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
                CommentAuthorIp = createDto.CommentAuthorIp  ,
                CommentApproved = "pending",
                CommentAgent = createDto.CommentAgent,
                CommentType= "comment",
                CommentParent=0,
            };
            var newComment = await _commentRepository.AddCommentAsync(comment);
            return newComment;
        }

        public async Task<bool> UpdateComment(ulong id, UpdateCommentDto updateDto)
        {
            return await _commentRepository.UpdateCommentAsync(id, updateDto);
        }

        public async Task<bool> DeleteComment(List<ulong> id)
        {
            return await _commentRepository.DeleteCommentAsync(id);
        }

        public async Task<bool> ApproveCommentAsync(ulong commentId)
        {
            return await _commentRepository.UpdateCommentStatusAsync(commentId, "approved");
        }

        public async Task<bool> DisapproveCommentAsync(ulong commentId)
        {
            return await _commentRepository.UpdateCommentStatusAsync(commentId, "hold");
        }

        public async Task<bool> MarkAsSpamAsync(ulong commentId)
        {
            return await _commentRepository.UpdateCommentStatusAsync(commentId, "spam");
        }

        public async Task<bool> UnspamCommentAsync(ulong commentId)
        {
            return await _commentRepository.UpdateCommentStatusAsync(commentId, "approved");
        }

        public async Task<WpComment> ReplyToCommentAsync(ReplyRequest request)
        {
            var replyComment = new WpComment
            {
                CommentPostId = request.PostId,
                CommentParent=request.ParentCommentId,
                CommentAuthor = request.Author,
                CommentAuthorEmail=request.AuthorEmail,
                CommentContent=request.Content,
                CommentApproved="aprooved",
                CommentDate = DateTime.UtcNow,
                CommentDateGmt = DateTime.UtcNow,
            };
            var newComment = await _commentRepository.AddCommentAsync(replyComment);
            return newComment;
        }
    }
}
