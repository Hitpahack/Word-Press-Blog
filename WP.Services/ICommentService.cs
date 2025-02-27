using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.Data;
using WP.DTOs;

namespace WP.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<WpComment>> GetAllCommentsAsync();
        Task<WpComment> GetCommentById(ulong id);
        Task<WpComment> AddComment(CreateCommentDto createDto);
        Task<bool> UpdateComment(ulong id, UpdateCommentDto updateDto);
        Task<bool> DeleteComment(List<ulong> id);
        Task<bool> ApproveCommentAsync(ulong commentId);
        Task<bool> DisapproveCommentAsync(ulong commentId);
        Task<bool> MarkAsSpamAsync(ulong commentId);
        Task<bool> UnspamCommentAsync(ulong commentId);
        Task<WpComment> ReplyToCommentAsync( ReplyRequest request);

    }
}
