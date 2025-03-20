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
}
