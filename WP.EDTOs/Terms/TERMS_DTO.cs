using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WP.EDTOs.Categories
{
    public class BASE_TERMS_DTO
    {
        public ulong? Term_Id { get; set; }
        public string? Name { get; set; }
        public string? Taxonomy { get; set; }
        public ulong? Term_Taxonomy_Id { get; set; }
        public string? Slug { get; set; }
        public ulong? Post_Count { get; set; }
        public ulong? Post_Id { get; set; }
        public ulong? Parent_Id { get; set; }
    }
    public class CATEGORIES_TERMS_DTO : BASE_TERMS_DTO
    {
        public string? Most_Used_Category { get; set; }
        public bool IsChecked { get; set; }
        [NotMapped]
        public List<CATEGORIES_TERMS_DTO> Subcategory { get; set; }

    }
    public class TAGS_TERMS_DTO : BASE_TERMS_DTO
    {
        public string? Most_Used_Tag { get; set; }

    }
}
