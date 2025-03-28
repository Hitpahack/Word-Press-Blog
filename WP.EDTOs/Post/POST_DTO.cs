using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WP.EDTOs.Categories;

namespace WP.EDTOs
{
    public class BASE_POST_SP_RESPONSE
    {
        [Required]
        [DisplayName("Title")]
        public string Post_Title { get; set; }
        [Required]
		[DisplayName("Name")]
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
    
    public class POST_DTO :BASE_POST_SP_RESPONSE
    {
        public POST_DTO()
        {
            Seo = new SEO_DTO();
        }
        public ulong? Id { get; set; }
        [NotMapped]
        [ValidateNever]
        public List<CATEGORIES_TERMS_DTO> CategoriesItems { get; set; }
        [NotMapped]
        [ValidateNever]
        public List<TAGS_TERMS_DTO> TagsItem { get; set; }
        [NotMapped]
        public string? NewCategory { get; set; }
        [NotMapped]
        public string? ParentCatId { get; set; }
        
        [NotMapped]
        public string? Slug { get; set; }
        [NotMapped]
        public string? Meta_Description { get; set; }
        [NotMapped]
        [ValidateNever]
        public IFormFile? FeaturedImage { get; set; }
        [NotMapped]
        public SEO_DTO Seo { get; set; }
    }
    public class SEO_DTO
    {
        [DisplayName("Keyphrase")]
        public string? Keyphrase { get; set; }
        [DisplayName("SEO Title")]
        public string? Seo_Title { get; set; }
        [DisplayName("Slug")]
        public string? Seo_Slug { get; set; }
        [DisplayName("Meta description")]
        public string? Seo_Meta_description { get; set; }
        [DisplayName("Page Type")]
        public string? Seo_Page_Type { get; set; }
        [DisplayName("Article Type")]
        public string? Seo_Article_Type { get; set; }
        public string? ReadabilityScore { get; set; }
        public string? SeoScore { get; set; }
    }

    public class SEO_SCORE_DTO
    {
        public SEO_SCORE_DTO(string readabilityscore="", string seoscore = "")
        {
            ReadabilityScore = readabilityscore;
            SeoScore = seoscore;
        }

        public string ReadabilityScore { get; set; }
        public string SeoScore { get; set; }

    }
    public class WP_POST_ADD_DTO : POST_DTO
    {
        public List<ulong> Categories { get; set; } = new();
        public List<ulong> Tags { get; set; } = new();
        public string? FeaturedImageUrl { get; set; }

    }
    public class WP_PAGE_ADD_DTO : POST_DTO
    {
        public string? FeaturedImageUrl { get; set; }

    }

   

}
