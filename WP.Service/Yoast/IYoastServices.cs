using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WP.Service.Yoast
{
    public interface IYoastServices : IDisposable
    {
        Dictionary<string, int> GetKeywordDensity(string content);
    }
    public class YoastServices : BaseServices, IYoastServices
    {
        private  readonly HashSet<string> StopWords = new HashSet<string>
        {
            "the", "and", "end", "these", "those", "is", "are", "was", "were", "this", "that", "to", "in", "on", "for", "with", "as", "of", "at", "by", "an", "a", "it", "has", "you", "am", "he", "she"
        };
        public Dictionary<string, int> GetKeywordDensity(string content)
        {
            var words = Regex.Split(content.ToLower(), @"\W+")
                             .Where(w => w.Length > 2 && !StopWords.Contains(w))
                             .GroupBy(w => w)
                             .ToDictionary(g => g.Key, g => g.Count());

            return words.OrderByDescending(k => k.Value).Take(10).ToDictionary(k => k.Key, k => k.Value);
        }
        public void Dispose()
        {
            
        }
    }

    
}
