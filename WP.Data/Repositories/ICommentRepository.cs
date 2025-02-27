using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.DTOs;

namespace WP.Data.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<WpComment>> GetAllCommentsAsync();
        Task<WpComment> GetCommentByIdAsync(ulong id);
        Task<WpComment> AddCommentAsync(WpComment comment);
        Task<bool> UpdateCommentAsync(ulong id, UpdateCommentDto updateDto);
        Task<bool> DeleteCommentAsync(List<ulong> id);
        Task<bool> UpdateCommentStatusAsync(ulong commentId, string status);
       
    }
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogContext _dbContext;

        public CommentRepository(BlogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WpComment> AddCommentAsync(WpComment comment)
        {
            _dbContext.WpComments.Add(comment);
            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> DeleteCommentAsync(List<ulong> id)
        {
            var commentToDelete = await _dbContext.WpComments.Where(c => id.Contains(c.CommentId)).ToListAsync();
            if (commentToDelete == null) return false;
            foreach (var comment in commentToDelete)
            {
                comment.CommentApproved = "trash";
            }
            _dbContext.WpComments.UpdateRange(commentToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WpComment>> GetAllCommentsAsync()
        {
            return await _dbContext.WpComments.ToListAsync();
        }

        public async Task<WpComment> GetCommentByIdAsync(ulong id)
        {
            return await _dbContext.WpComments.FindAsync(id);
        }

        public async Task<bool> UpdateCommentAsync(ulong id, UpdateCommentDto updateDto)
        {
            var comment = await _dbContext.WpComments.FindAsync(id);
            if (comment == null) return false;

            comment.CommentContent = updateDto.CommentContent;
            comment.CommentDate = updateDto.CommentDate;
            comment.CommentAuthorEmail = updateDto.CommentAuthorEmail;
            comment.CommentAuthor = updateDto.CommentAuthor;
            comment.CommentAuthorUrl = updateDto.CommentAuthorUrl;
            comment.CommentApproved = updateDto.CommentApproved;

            _dbContext.WpComments.Update(comment);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCommentStatusAsync(ulong commentId, string status)
        {
            var comment = await _dbContext.WpComments.FindAsync(commentId);
            if (comment == null) return false;

            comment.CommentApproved = status;
            _dbContext.WpComments.Update(comment);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }

}
