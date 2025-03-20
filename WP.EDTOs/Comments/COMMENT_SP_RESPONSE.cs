using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.EDTOs.Comments
{
    public class COMMENT_SP_RESPONSE: SP_BASE_RESPONSE
    {
            public ulong Comment_Id { get; set; }
            public ulong Comment_Post_Id { get; set; }
            public string? Post_Title { get; set; }
            public string Comment_Author { get; set; } 
            public string Comment_Author_Email { get; set; }
            public string? Comment_Author_Url { get; set; }
            public string? Comment_Content { get; set; }
            public DateTime? Comment_Date_Gmt { get; set; }
            public string? Comment_Approved { get; set; }
            public string Avatar { get; set; }


    }
    public class EditCommentDto
    {
        public ulong CommentId { get; set; }
        public string CommentAuthor { get; set; }
        public string CommentAuthorEmail { get; set; }
        public string? CommentAuthorUrl { get; set; }
        public string CommentContent { get; set; } 
        public string? Post_Title { get; set; }
        public string CommentApproved { get; set; }
    }

    public class ReplyCommentDto
    {
            public ulong CommentId { get; set; }
            public string ReplyContent { get; set; }

    }

    public class WpCommentDto
    {
        public ulong CommentId { get; set; }

        public ulong CommentPostId { get; set; }

        public string CommentAuthor { get; set; }

        public string CommentAuthorEmail { get; set; } 

        public string CommentAuthorUrl { get; set; } 

        public string CommentAuthorIp { get; set; } 

        public DateTime CommentDate { get; set; }

        public DateTime CommentDateGmt { get; set; }

        public string CommentContent { get; set; }

        public int CommentKarma { get; set; }

        public string CommentApproved { get; set; } 

        public string CommentAgent { get; set; }

        public string CommentType { get; set; } 

        public ulong CommentParent { get; set; }

        public ulong UserId { get; set; }
    }

}
