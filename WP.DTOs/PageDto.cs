using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.DTOs
{
    public class PageDto
    {
        public ulong Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public ulong AuthorId { get; set; }
        public ulong? ParentId { get; set; }
    }
    public class CreatePageDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; } = "";
        public string Status { get; set; } = "publish";  // publish, draft, pending
        public ulong AuthorId { get; set; } = 1;  // Default: Admin User ID
        public ulong ParentId { get; set; } = 0;  // Default: No parent (Top-level page)
    }
    public class UpdatePageDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public string Status { get; set; }
        public ulong ParentId { get; set; }
    }


}
