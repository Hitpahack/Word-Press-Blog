using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.EDTOs.Comments
{
    public class UpdateCommentStatusRequest
    {
        public List<ulong> CommentIds { get; set; }
        public string Status { get; set; } // "approve", "spam", "trash", etc.
    }
}
