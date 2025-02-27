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
}
