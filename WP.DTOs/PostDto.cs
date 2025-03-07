using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.DTOs
{
    public class PostDto
    {
        public ulong Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Excerpt { get; set; }
        public string Status { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string Author { get; set; }
        public ulong PostAuthor { get; set; }
        public IEnumerable<string> Categories { get; set; } 
        public IEnumerable<string> Tags { get; set; } 
        public string FeaturedImage { get; set; }
    }

    public class DataTableResponse<T> where T : class
    {
        public DataTableResponse(IEnumerable<T> data, int draw, int totalitem, int filteritem)
        {
            Draw = draw;
            RecordsTotal = totalitem;
            RecordsFiltered = filteritem;
            Data = data;
        }
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
    public class CreatePostDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string? Excerpt { get; set; }
        public string Status { get; set; } = "publish";  // publish, draft, pending
        public ulong AuthorId { get; set; }  // Default to admin
        public List<ulong> Categories { get; set; } = new(); // Category IDs
        public List<ulong> Tags { get; set; } = new(); // Tag IDs
        public string? FeaturedImageUrl { get; set; }  // Optional Featured Image
        
    }

    public class UpdatePostDto
    {
        public string Title { get; set; }  // Optional
        public string Content { get; set; }  // Optional
        public string Excerpt { get; set; }  // Optional
        public string Status { get; set; }  // Optional (publish, draft, pending, etc.)
        public List<ulong> Categories { get; set; }  // Optional Category IDs
        public List<ulong> Tags { get; set; }  // Optional Tag IDs
        public string FeaturedImageUrl { get; set; }  // Optional Featured Image URL
    }

    
}

