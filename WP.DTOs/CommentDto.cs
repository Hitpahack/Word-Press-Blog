using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.DTOs
{
    public class CommentDto
    {
        public ulong CommentId { get; set; }
        public ulong CommentPostId { get; set; }
        public string CommentAuthor { get; set; } = null!;
        public string CommentContent { get; set; } = null!;
        public DateTime CommentDate { get; set; }
    }
    public class CreateCommentDto
    {
        public ulong CommentPostId { get; set; }
        public string CommentAuthor { get; set; } = null!;
        public string CommentAuthorEmail { get; set; } = null!;
        public string CommentContent { get; set; } = null!;
        public ulong UserId { get; set; }
        public string CommentAuthorIp { get; set; } = null!;
        public string CommentAgent { get; set; } = null!;

    }
    public class UpdateCommentDto
    {
        public string CommentAuthor { get; set; } = null!;
        public string CommentContent { get; set; } = null!;
        public string CommentAuthorEmail { get; set; } = null!;
        public string CommentAuthorUrl { get; set; } = null!;
        public DateTime CommentDate { get; set; }
        public string CommentApproved { get; set; } = null!;

    }
    public class ReplyRequest
    {
        public ulong PostId { get; set; }
        public ulong ParentCommentId { get; set; }
        public string Author { get; set; }
        public string AuthorEmail { get; set; }
        public string Content { get; set; }
    }




}
