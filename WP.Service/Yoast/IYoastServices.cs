using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WP.EDTOs.Yoast;

namespace WP.Service.Yoast
{
    public interface IYoastServices : IDisposable
    {
        Task<Dictionary<string, int>> FocusKeyphrase(string content);
        Task<List<YOAST_DTO>> Readability(string content);
    }
    public class YoastServices : BaseServices, IYoastServices
    {
        private  readonly HashSet<string> StopWords = new HashSet<string>
        {
            "the", "and", "end", "these", "those", "is", "are", "was", "were", "this", "that", "to", "in", "on", "for", "with", "as", "of", "at", "by", "an", "a", "it", "has", "you", "am", "he", "she"
        };
        public Task<Dictionary<string, int>> FocusKeyphrase(string content)
        {
            var words = Regex.Split(content.ToLower(), @"\W+")
                             .Where(w => w.Length > 2 && !StopWords.Contains(w))
                             .GroupBy(w => w)
                             .ToDictionary(g => g.Key, g => g.Count());

            return Task.FromResult(words.OrderByDescending(k => k.Value).Take(10).ToDictionary(k => k.Key, k => k.Value));
        }

        public async Task<List<YOAST_DTO>> Readability(string content)
        {
            List<YOAST_DTO> result = new List<YOAST_DTO>();
            if (string.IsNullOrEmpty(content))
            {
                result.AddRange([new YOAST_DTO
                {
                    title = "Word complexity",
                    description = "Is your vocabulary suited for a larger audience?",
                    url = "https://yoa.st/word-complexity-metabox?php_version=8.2&platform=wordpress&platform_version=6.7.2&software=free&software_version=24.6&days_active=549&user_language=en_US&context=classic-metabox",
                    resultflat = "Problems"

                }, new YOAST_DTO 
                {
                    title = "Not enough content",
                    result_message = "Please add some content to enable a good analysis",
                    resultflat = "Problems"

                }]);
            }
            else
            {
               await Task.WhenAll(
                   Task.Run(async ()=>{
                       bool isComplex = await IsComplexWord(content);
                       if (isComplex) 
                       {
                           result.Add(new YOAST_DTO
                           {
                               title = "Word complexity",
                               resultflat = "problems",
                               result_message = "your vocabulary suited for a larger audience"
                           });
                       }
                    }), 
                   Task.Run(() =>
                   {
                       int totalSentences = CountSentences(content);
                       int passiveCount = CountPassiveVoiceSentences(content);
                       double percentage = (passiveCount / (double)totalSentences) * 100;
                       if (totalSentences == 0) 
                       {
                           result.Add(new YOAST_DTO
                           {
                               title = "Passive voice",
                               resultflat = "good",
                               result_message = "There is no sentences contain passive voice."
                           });
                       }
                       else
                       {
                           result.Add(new YOAST_DTO
                           {
                               title = "Passive voice",
                               resultflat = percentage < 10 ? "Good" :
                                          percentage <= 15 ? "Needs Improvement" :
                                                             "Problems",
                               result_message = percentage < 10 ? "You're using enough active voice. That's great!" : $"{percentage}% of the sentences contain passive voice, which is more than the recommended maximum of 10%",
                               url = "https://yoa.st/34t?php_version=8.2&platform=wordpress&platform_version=6.7.2&software=free&software_version=24.6&days_active=549&user_language=en_US"
                           });
                       }
                   
                   }),
                   Task.Run(() =>
                   {
                       int total = CheckSubheadingDistribution(content).Count;
                       if(total == 0)
                       {
                           result.Add(new YOAST_DTO
                           {
                               title = "Subheading distribution",
                               resultflat = "good",
                               result_message = "Great job!"
                           });
                       }
                       else
                       {

                       }
                       "You are not using any subheadings, although your text is rather long"
                   }));
                
                //bool iscomplexword = IsComplexWord(content);
            }
            return Task.FromResult(result);
        }

        #region Readability
        public Task<bool> IsComplexWord(string word)
        {
            string[] commonWords = { "the", "is", "and", "you", "that", "this", "it", "not" }; // Basic word list
            return Task.FromResult(!commonWords.Contains(word.ToLower()) && word.Length > 6);
        }

        private readonly List<string> SingleWordTransitions = new List<string>
        {
            "accordingly","additionally","afterward","afterwards","albeit","also","although","altogether","another","basically","because","before","besides","but","certainly","chiefly","comparatively","concurrently","consequently","contrarily","conversely","correspondingly","despite","doubtedly","during","e.g.","earlier","emphatically","equally","especially","eventually","evidently","explicitly","finally","firstly","following","formerly","forthwith","fourthly","further","furthermore","generally","hence","henceforth","however","i.e.","identically","indeed","instead","last","lastly","later","lest","likewise","markedly","meanwhile","moreover","nevertheless","nonetheless","nor","notwithstanding","obviously","occasionally","otherwise","once","overall","particularly","presently","previously","rather","regardless","secondly","shortly","significantly","similarly","simultaneously","since","so","soon","specifically","still","straightaway","subsequently","surely","surprisingly","than","then","thereafter","therefore","thereupon","thirdly","though","thus","till","undeniably","undoubtedly","unless","unlike","unquestionably","until","when","whenever","whereas","while","weil","doch","mit anderen worten","so dass","omdat","maar","net als","ter conclusie","car","toutefois","si bien que","en raison de","porque","pero","a causa de","sin embargo","perché","però","a causa","in sentesi","pois","contudo","por causa de","em suma","потому","однако","потому что","в итоге","ponieważ","jednak","z uwagi że","w podsumowaniu","perquè","resumint","pel que","en a resum","emellertid","men","i syfte att","för att sammanfatta","mivel","azonban","ahhoz hogy","más szóval","بينما","حيثما","هكذا","كذلك","كما","למרות","בשביל","כגון","מלבד","מפאת","berikut","kedua","terutamanya","terdahulu","contohnya","fakat","ama","çünkü","yüzünden","topyekun","だから","そのため","第一に","具体的には"
        };
        private readonly List<string> MultiWordTransitions = new List<string>
        {
            "above all","after all","after that","all in all","all of a sudden","all things considered","analogous to","although this may be true","another key point","as a matter of fact","as a result","as an illustration","as can be seen","as has been noted","as I have noted","as I have said","as I have shown","as long as","as much as","as shown above","as soon as","as well as","at any rate","at first","at last","at least","at length","at the present time","at the same time","at this instant","at this point","at this time","balanced against","being that","by all means","by and large","by comparison","by the same token","by the time","compared to","be that as it may","coupled with","different from","due to","equally important","even if","even more","even so","even though","first thing to remember","for example","for fear that","for instance","for one thing","for that reason","for the most part","for the purpose of","for the same reason","for this purpose","for this reason","from time to time","given that","given these points","important to realize","once in a while","in a word","in addition","in another case","in any case","in any event","in brief","in case","in conclusion","in contrast","in detail","in due time","in effect","in either case","in essence","in fact","in general","in light of","in like fashion","in like manner","in order that","in order to","in other words","in particular","in reality","in short","in similar fashion","in spite of","in sum","in summary","in that case","in the event that","in the final analysis","in the first place","in the fourth place","in the hope that","in the light of","in the long run","in the meantime","in the same fashion","in the same way","in the second place","in the third place","in this case","in this situation","in time","in truth","in view of","most compelling evidence","most important","must be remembered","not to mention","now that","of course","on account of","on balance","on condition that","on one hand","on the condition that","on the contrary","on the negative side","on the other hand","on the positive side","on the whole","on this occasion","only if","owing to","point often overlooked","prior to","provided that","seeing that","so as to","so far","so long as","so that","sooner or later","such as","summing up","take the case of","that is","that is to say","then again","this time","to be sure","to begin with","to clarify","to conclude","to demonstrate","to emphasize","to enumerate","to explain","to illustrate","to list","to point out","to put it another way","to put it differently","to repeat","to rephrase it","to say nothing of","to sum up","to summarize","to that end","to the end that","to this end","together with","under those circumstances","until now","up against","up to the present time","vis a vis","what’s more","while it may be true","while this may be true","with attention to","with the result that","with this in mind","with this intention","with this purpose in mind","with attention to","with the result that","with this in mind","with this intention","with this purpose in mind","without a doubt","without delay","without doubt","without reservation","علاوة على","من ناحية أخرى","على سبيل المثال","في النهاية","بناء على ذلك","לא כל שכן","כמו כן","בזמן האחרון","לטווח ארוך","לא כל שכן","berbeda dari","kendatipun begitu","dengan pemikiran ini","pada waktu","seperti yang sudah dijelaskan","demek ki","farz edelim ki","dolayısı ile","bunun yanı sıra","kısaca söylecek olursak"
        };
        private readonly List<(string, string)> TwoPartTransitions = new List<(string, string)>
            {
                ("both", "and"),
                ("if", "then"),
                ("not", "only"),
                ("but", "also"),
                ("neither", "nor"),
                ("whether", "or"),
                ("both", "and")
            };

        public  double GetTransitionWordPercentage(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return 0;

            // Normalize content
            content = content.Trim().ToLower();

            // Split into sentences
            string[] sentences = Regex.Split(content, @"(?<=[.!?])\s+")
                                      .Where(s => !string.IsNullOrWhiteSpace(s))
                                      .ToArray();

            int transitionSentenceCount = 0;

            foreach (var sentence in sentences)
            {
                string trimmedSentence = sentence.Trim();

                // Check for single-word transitions
                if (SingleWordTransitions.Any(word =>
                    Regex.IsMatch(trimmedSentence, $@"\b{Regex.Escape(word)}\b", RegexOptions.IgnoreCase)))
                {
                    transitionSentenceCount++;
                    continue;
                }

                // Check for multi-word transitions
                if (MultiWordTransitions.Any(phrase =>
                    Regex.IsMatch(trimmedSentence, $@"\b{Regex.Escape(phrase)}\b", RegexOptions.IgnoreCase)))
                {
                    transitionSentenceCount++;
                    continue;
                }

                // Check for two-part transitions
                foreach (var (firstPart, secondPart) in TwoPartTransitions)
                {
                    if (Regex.IsMatch(trimmedSentence, $@"\b{Regex.Escape(firstPart)}\b.*\b{Regex.Escape(secondPart)}\b", RegexOptions.IgnoreCase))
                    {
                        transitionSentenceCount++;
                        break;
                    }
                }
            }

            return sentences.Length > 0 ? (double)transitionSentenceCount / sentences.Length * 100 : 0;
        }
        public  List<int> CheckSubheadingDistribution(string content)
        {
            var paragraphs = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<int> longParagraphs = new List<int>();

            for (int i = 0; i < paragraphs.Length; i++)
            {
                if (paragraphs[i].Split(' ').Length > 300)
                {
                    longParagraphs.Add(i + 1); // Paragraph index (1-based)
                }
            }
            return longParagraphs;
        }

        private  readonly Dictionary<string, string> PassiveToActive = new Dictionary<string, string>
        {
            { @"\bis\b (.*?)ed\b by\b", "I $1" },  // Present simple passive → active
            { @"\bis being\b (.*?)ed\b by\b", "I am $1" },  // Present continuous passive → active
            { @"\bhas been\b (.*?)ed\b by\b", "I have $1" },  // Present perfect passive → active
            { @"\bwas\b (.*?)ed\b by\b", "I $1" },  // Past simple passive → active
            { @"\bwas being\b (.*?)ed\b by\b", "I was $1" },  // Past continuous passive → active
            { @"\bhad been\b (.*?)ed\b by\b", "I had $1" },  // Past perfect passive → active
            { @"\bwill be\b (.*?)ed\b by\b", "I will $1" },  // Future passive → active
            { @"\bwill have been\b (.*?)ed\b by\b", "I will have $1" }  // Future perfect passive → active
        };
        
        public  int CountPassiveVoiceSentences(string content)
        {
            // Improved passive voice pattern with optional adverbs
            string passivePattern = @"\b(is|was|were|are|been|being|be|has been|have been|had been|will be|shall be|can be|could be|should be|might be)\s+(\w+\s+)?\w+ed\b";

            // Additional pattern to capture passive structures like "is bound to", "is considered", etc.
            string additionalPattern = @"\b(is|was|were|are|has been|have been|had been)\s+(considered|known|believed|said|thought|expected|meant|intended|bound)\b";

            // Splitting sentences based on punctuation
            string[] sentences = content.Split(new[] { ".", "!", "?" }, StringSplitOptions.RemoveEmptyEntries);
            var passivsentence = sentences.Where(sentence => Regex.IsMatch(sentence, passivePattern, RegexOptions.IgnoreCase));
            // Count sentences that match the passive voice pattern
            int passiveCount = sentences.Count(sentence =>
                Regex.IsMatch(sentence, passivePattern, RegexOptions.IgnoreCase) ||
                Regex.IsMatch(sentence, additionalPattern, RegexOptions.IgnoreCase));

            return passiveCount;




        }

        public  (int, double, string) AnalyzePassiveVoice(string content)
        {
            int totalSentences = CountSentences(content);
            int passiveCount = CountPassiveVoiceSentences(content);

            if (totalSentences == 0) return (0, 0, "✅ Good");

            double percentage = (passiveCount / (double)totalSentences) * 100;

            string indicator = percentage < 10 ? "✅ Good" :
                               percentage <= 15 ? "⚠️ Warning" :
                                                  "❌ Needs Improvement";

            return (passiveCount, percentage, indicator);
        }

        public  int CountSentences(string content)
        {
            string[] sentences = content.Split(new[] { ".", "!", "?" }, StringSplitOptions.RemoveEmptyEntries);
            return sentences.Length;
        }

        public  bool HasRepetitiveSentences(string content)
        {
            string[] sentences = content.Split(new[] { ".", "!", "?" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < sentences.Length - 2; i++)
            {
                string firstWord1 = sentences[i].Trim().Split(' ')[0];
                string firstWord2 = sentences[i + 1].Trim().Split(' ')[0];
                string firstWord3 = sentences[i + 2].Trim().Split(' ')[0];

                if (firstWord1.Equals(firstWord2, StringComparison.OrdinalIgnoreCase) &&
                    firstWord2.Equals(firstWord3, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public  bool AreParagraphsTooLong(string content)
        {
            var paragraphs = content.Split(new[] { "\n\n", " " }, StringSplitOptions.RemoveEmptyEntries);
            return paragraphs.Any(p => p.Split(' ').Length > 200);
        }

        public  double GetAverageSentenceLength(string content)
        {
            string[] sentences = content.Split(new[] { ".", "!", "?" }, StringSplitOptions.RemoveEmptyEntries);
            int totalWords = sentences.Sum(sentence => sentence.Split(' ').Length);

            return (double)totalWords / sentences.Length;
        }

        public  void AnalyzeContent(string content)
        {
            var result = AnalyzePassiveVoice(content);

            Console.WriteLine("🔹 Word Complexity: " + (IsComplexWord(content) ? "Needs Improvement" : "Good"));
            Console.WriteLine("🔹 Subheading Distribution Issues: " + CheckSubheadingDistribution(content).Count);
            Console.WriteLine("🔹 Transition Words: " + GetTransitionWordPercentage(content) + "% (Ideal: 30%+)");
            Console.WriteLine("🔹 Passive Voice Sentences: " + CountPassiveVoiceSentences(content));
            Console.WriteLine("🔹 Repetitive (Consecutive) Sentences: " + (HasRepetitiveSentences(content) ? "Yes" : "No"));
            Console.WriteLine("🔹 Paragraph Length: " + (AreParagraphsTooLong(content) ? "Too long" : "Good"));
            Console.WriteLine("🔹 Average Sentence Length: " + GetAverageSentenceLength(content) + " words (Ideal: <25)");
        }
        #endregion






        public void Dispose()
        {
            
        }
    }

    
}
