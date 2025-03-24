using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WP.EDTOs.Yoast
{
    public class YOAST_DTO
    {
        public string title { get; set; }
        public string resultflag { get; set; } // good, improvements, problems
        public string result_message { get; set; } 
        public string url { get; set; } 
        public string description { get; set; } 
        public string linkHtml { get; set; } 
    }
    public enum result_flag
    {
        problems,
        good,
        Need_Improvement
    }

   public class SEOAnalyzer
    {
        public SEOAnalyzer()
        {
           
        }

        public string FocusKeyphrase { get; set; }
        public string SeoTitle { get; set; }
        public string Slug { get; set; }
        public string MetaDescription { get; set; }
        public string Content { get; set; }
        public List<string> Subheadings { get; set; }
        public List<string> ImageSources { get; set; }
        public List<string> ImageAltTexts { get; set; }
        public SEOAnalyzer(string focusKeyphrase, string seoTitle, string slug, string metaDescription, string content)
        {
            FocusKeyphrase = focusKeyphrase.ToLower();
            SeoTitle = seoTitle.ToLower();
            Slug = slug.ToLower();
            MetaDescription = metaDescription.ToLower();
            Content = content.ToLower();
            
        }
        public Dictionary<string, Dictionary<string, string>> Analyze()
        {
            var good = new Dictionary<string, string>();
            var problems = new Dictionary<string, string>();
            var improvements = new Dictionary<string, string>();
            Subheadings = ExtractSubheadings();
            
            int wordCount = GetWordCount(Content);
            int keyphraseCount = CountOccurrences(Content, FocusKeyphrase);

            // Outbound Links Check
            if (Content.Contains("<a href="))
                good["Outbound Links"] = "Good job!";
            else
                improvements["Outbound Links"] = "Consider adding outbound links.";

            // Internal Links Check
            if (Content.Contains("href=\"/"))
                good["Internal Links"] = "You have enough internal links. Good job!";
            else
                improvements["Internal Links"] = "Consider adding internal links.";

            // Keyphrase Density
            if (keyphraseCount >= 3 && keyphraseCount <= 10)
                good["Keyphrase Density"] = $"The keyphrase was found {keyphraseCount} times. This is great!";
            else
                problems["Keyphrase Density"] = $"The keyphrase was found {keyphraseCount} times. Try optimizing density.";

            // Keyphrase in Meta Description
            if (MetaDescription.Contains(FocusKeyphrase, StringComparison.OrdinalIgnoreCase))
                good["Keyphrase in Meta Description"] = "Keyphrase appears in the meta description. Well done!";
            else
                problems["Keyphrase in Meta Description"] = "Add the keyphrase to the meta description.";

            // Meta Description Length
            if (MetaDescription.Length >= 120 && MetaDescription.Length <= 160)
                good["Meta Description Length"] = "Well done!";
            else
                problems["Meta Description Length"] = "Meta description should be 120-160 characters.";

            // Text Length
            if (wordCount >= 300)
                good["Text Length"] = $"The text contains {wordCount} words. Good job!";
            else
                problems["Text Length"] = $"Text is too short ({wordCount} words). Aim for at least 300.";

            // SEO Title Width
            if (SeoTitle.Length >= 50 && SeoTitle.Length <= 60)
                good["SEO Title Width"] = "Good job!";
            else
                problems["SEO Title Width"] = "SEO title should be between 50-60 characters.";

            // Keyphrase in SEO Title
            if (SeoTitle.Contains(FocusKeyphrase, StringComparison.OrdinalIgnoreCase))
                good["Keyphrase in SEO Title"] = "Keyphrase appears in the SEO title. Well done!";
            else
                problems["Keyphrase in SEO Title"] = "Include the keyphrase in the SEO title.";

            // Previously Used Keyphrase (Mocked Check)
            bool previouslyUsed = false; // Simulating a check
            if (!previouslyUsed)
                good["Previously Used Keyphrase"] = "You've not used this keyphrase before. Very good.";
            else
                improvements["Previously Used Keyphrase"] = "You've used this keyphrase before. Try a variation.";

            return new Dictionary<string, Dictionary<string, string>>
            {
               { "good", good},
               { "problems", problems},
               { "improvement", improvements}
            };
        }

        private int GetWordCount(string text)
        {
            return text.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        private int CountOccurrences(string text, string word)
        {
            return text.ToLower().Split(new[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                .Count(w => w == word.ToLower());
        }

        private List<string> ExtractSubheadings()
        {
            var subheadings = new List<string>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(Content);

            foreach (var heading in htmlDoc.DocumentNode.SelectNodes("//h2 | //h3 | //h4") ?? new HtmlNodeCollection(null))
            {
                subheadings.Add(heading.InnerText.Trim());
            }
            return subheadings;
        }

        private (List<string> ImageSources, List<string> ImageAltTexts) ExtractImages(string htmlContent)
        {
            var imageSources = new List<string>();
            var imageAltTexts = new List<string>();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            foreach (var img in htmlDoc.DocumentNode.SelectNodes("//img") ?? new HtmlNodeCollection(null))
            {
                var src = img.GetAttributeValue("src", "");
                var alt = img.GetAttributeValue("alt", "");

                if (!string.IsNullOrEmpty(src)) imageSources.Add(src);
                if (!string.IsNullOrEmpty(alt)) imageAltTexts.Add(alt);
            }
            return (imageSources, imageAltTexts);
        }

    }
}
