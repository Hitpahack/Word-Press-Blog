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
        public string FocusKeyphrase { get; set; }
        public string SeoTitle { get; set; }
        public string Slug { get; set; }
        public string MetaDescription { get; set; }
        public string Content { get; set; }
        public List<string> Subheadings { get; set; }

        public SEOAnalyzer(string focusKeyphrase, string seoTitle, string slug, string metaDescription, string content, List<string> subheadings)
        {
            FocusKeyphrase = focusKeyphrase.ToLower();
            SeoTitle = seoTitle.ToLower();
            Slug = slug.ToLower();
            MetaDescription = metaDescription.ToLower();
            Content = content.ToLower();
            Subheadings = subheadings.Select(s => s.ToLower()).ToList();
        }

        public Dictionary<string, List<string>> Analyze()
        {
            var results = new Dictionary<string, List<string>>
        {
            { "Needs Improvement", new List<string>() },
            { "Improvements", new List<string>() },
            { "Good Results", new List<string>() }
        };

            // Check Keyphrase Distribution
            int keyphraseCount = Regex.Matches(Content, $"\\b{Regex.Escape(FocusKeyphrase)}\\b").Count;
            if (keyphraseCount < 3)
                results["Needs Improvement"].Add("Keyphrase distribution: Use the keyphrase more evenly throughout the text.");
            else
                results["Good Results"].Add($"Keyphrase density: The keyphrase was found {keyphraseCount} times. This is great!");

            // Check Keyphrase in Introduction
            string firstParagraph = Content.Split(new[] { "\n", ". " }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            if (firstParagraph != null && !firstParagraph.Contains(FocusKeyphrase))
                results["Needs Improvement"].Add("Keyphrase in introduction: Your keyphrase does not appear in the first paragraph. Make sure the topic is clear immediately.");

            // Check Keyphrase in Subheadings
            if (!Subheadings.Any(sh => sh.Contains(FocusKeyphrase)))
                results["Needs Improvement"].Add("Keyphrase in subheading: Use more keyphrases or synonyms in your H2 and H3 subheadings!");

            // Check Keyphrase in SEO Title
            if (!SeoTitle.StartsWith(FocusKeyphrase))
                results["Improvements"].Add("Keyphrase in SEO title: The focus keyphrase appears in the SEO title, but not at the beginning. Move it to the beginning for the best results.");
            else
                results["Good Results"].Add("Keyphrase in SEO title: Good job!");

            // Check Keyphrase in Slug
            if (!Slug.Contains(FocusKeyphrase))
                results["Improvements"].Add("Keyphrase in slug: Your keyphrase does not appear in the slug. Change that!");
            else
                results["Good Results"].Add("Keyphrase in slug: Good job!");

            // Check Keyphrase in Meta Description
            if (MetaDescription.Contains(FocusKeyphrase))
                results["Good Results"].Add("Keyphrase in meta description: Well done!");
            else
                results["Needs Improvement"].Add("Keyphrase in meta description: Your keyphrase is missing. Add it for better SEO.");

            // Meta Description Length Check
            if (MetaDescription.Length >= 50 && MetaDescription.Length <= 160)
                results["Good Results"].Add("Meta description length: Well done!");
            else
                results["Needs Improvement"].Add("Meta description length: Should be between 50-160 characters.");

            // Text Length Check
            int wordCount = Content.Split(new[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            if (wordCount >= 300)
                results["Good Results"].Add($"Text length: The text contains {wordCount} words. Good job!");
            else
                results["Needs Improvement"].Add("Text length: Your content is too short. Consider adding more information.");

            return results;
        }
    }
}
