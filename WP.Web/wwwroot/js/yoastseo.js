(function (global, $) {
    function YoastSEO(options) {
        var settings = $.extend({
            contentSelector: '',  // Required: Set the content input
            stopWords: ["the", "and", "end", "these", "those", "is", "are", "was", "were", "this", "that", "to", "in", "on", "for", "with", "as", "of", "at", "by", "an", "a", "it", "has", "you", "am", "he", "she"],
            focusKeyword: '',
            readability: true,
            keywordDensity: true
        }, options);

        var contentElement = $(settings.contentSelector);
        if (!contentElement.length) {
            console.warn("YoastSEO: contentSelector not found!");
            return;
        }

        var content = contentElement.val() || contentElement.text() || '';

        function getWordCount(_content, stopWords) {
            
            // Retrieve the token value
            var token = settings.antiForgeryToken;

            // Data to send
            var dataToSend = {
                content: _content
            };

            // Make the POST request
            $.post("/getfocuskeyphrase", dataToSend)
                .done(function (response) {
                    var resul = generateSEOKeyphrase(response);
                    analysisResults.topWords = resul;
                    //$.each(response, function (key, value) {
                    //    $('#keyword_result').append(`<li>${key} (${value})</li><br>`);
                    //});
                })
                .fail(function (xhr) {
                    alert("Error: " + xhr.responseText);
                });

        }

        function getReadabilityScore(content) {
            const sentences = content.split(/[.!?]+/).filter(s => s.length);
            const words = content.split(/\s+/).filter(w => w.length);
            const syllables = words.reduce((count, word) => count + countSyllables(word), 0);

            const sentenceCount = sentences.length || 1;
            const wordCount = words.length || 1;

            return Math.round(206.835 - (1.015 * (wordCount / sentenceCount)) - (84.6 * (syllables / wordCount)));
        }

        function countSyllables(word) {
            return word.toLowerCase().match(/[aeiouy]{1,2}/g)?.length || 1;
        }

        function getKeywordDensity(content, keyword) {
            if (!keyword) return 0;
            return (content.match(new RegExp(`\\b${keyword}\\b`, 'gi')) || []).length;
        }

        function generateSEOKeyphrase(response) {
            // Convert response object into an array of key-value pairs
            let wordEntries = Object.entries(response);

            // Sort words based on their score (higher first)
            wordEntries.sort((a, b) => b[1] - a[1]);

            // Determine a dynamic threshold for primary vs secondary
            let maxScore = wordEntries[0][1];  // Highest score in the list
            let threshold = maxScore * 0.5;  // Set threshold as 50% of the highest score

            let primaryKeywords = [];
            let secondaryKeywords = [];

            wordEntries.forEach(([word, score]) => {
                if (score >= threshold) {
                    primaryKeywords.push(word);
                } else {
                    secondaryKeywords.push(word);
                }
            });

            // Construct SEO-friendly keyphrase
            let keyphrase = primaryKeywords.join(" & ");

            if (secondaryKeywords.length > 0) {
                keyphrase += (keyphrase ? " - " : "") + secondaryKeywords.join(" | ");
            }

            return keyphrase || "Trending Topics & Styles"; // Fallback if no keywords found
        }
        var analysisResults = {
            topWords: getWordCount(content, settings.stopWords),
            readabilityScore: settings.readability ? getReadabilityScore(content) : null,
            keywordDensity: settings.keywordDensity ? getKeywordDensity(content, settings.focusKeyword) : null
        };

        return {
            keywordDensity: function (callback) {
                if (typeof callback === 'function') {
                    callback(analysisResults.keywordDensity);
                }
                return this;
            },
            readability: function (callback) {
                if (typeof callback === 'function') {
                    callback(analysisResults.readabilityScore);
                }
                return this;
            },
            focusKeyword: function (callback) {
                if (typeof callback === 'function') {
                    callback(analysisResults.topWords);
                }
                return this;
            }
        };
    }

    // Load YoastSEO globally
    global.yoastSEO = function (options) {
        return new YoastSEO(options);
    };
})(window, jQuery);
