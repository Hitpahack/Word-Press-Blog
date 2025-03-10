using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WP.EDTOs.Categories;

namespace WP.EDTOs
{
    public class BASE_SP_RESPONSE
    {
        [Required]
        public string Post_Title { get; set; }
        [Required]
        public string Post_Name { get; set; }
        [Required]
        public string Post_Content { get; set; }
        public string? Post_Status { get; set; }
        public DateTime? Post_Date { get; set; }
        public DateTime? Post_Date_Gmt { get; set; }
        public ulong? Post_Author { get; set; }
        public string? User_Login { get; set; }
        public string? featured_image_url { get; set; }
    }
    
    public class POST_DTO :BASE_SP_RESPONSE
    {
        public ulong Id { get; set; }
        [NotMapped]
        [ValidateNever]
        public List<CATEGORIES_TERMS_DTO> CategoriesItems { get; set; }
        [NotMapped]
        [ValidateNever]
        public List<TAGS_TERMS_DTO> TagsItem { get; set; } 
    }


    public class WP_POST_ADD_DTO : POST_DTO
    {
       
        public List<ulong> Categories { get; set; } = new();
        public List<ulong> Tags { get; set; } = new();
        public string? FeaturedImageUrl { get; set; }
        
    }



}
