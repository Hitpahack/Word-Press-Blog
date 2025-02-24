using System;
using System.Collections.Generic;
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
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public ulong Parent { get; set; }
    }


}
