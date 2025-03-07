using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.DTOs
{
    public class CategoryDto
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public ulong Parent { get; set; }

    }
    public class CategoryResponseDto
    {
        public ulong TermId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public long? Count { get; set; }
        public ulong TermTaxonomyId { get; set; }
    }

    public class CategoryRequestDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, ErrorMessage = "Category name must be between 2 and 50 characters.", MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Description must not exceed 200 characters.")]
        public string? Description { get; set; }
        public string Slug { get; set; }
        public ulong? Parent { get; set; }
    }


}
