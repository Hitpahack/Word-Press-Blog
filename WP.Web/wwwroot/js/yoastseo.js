(function (global, $) {
    function YoastSEO(options) {
        var settings = $.extend({
            contentSelector: '',  // Required: Set the content input
            editor: null,  // Required: Set the content input
            stopWords: ["the", "and", "end", "these", "those", "is", "are", "was", "were", "this", "that", "to", "in", "on", "for", "with", "as", "of", "at", "by", "an", "a", "it", "has", "you", "am", "he", "she"],
            delayTimeout: 3000,
            seo: {
                focusKeyphrase: '',
                seoTitle: '',
                slug: '',
                metaDescription: ''
            }
        }, options);

        let typingTimer; 
        let _htmlcontent; 
        if (!settings.editor)
            throw "editor required";

        var contentElement = $(settings.contentSelector);
        if (!contentElement.length) {
            console.warn("YoastSEO: contentSelector not found!");
            return;
        }

        var content = contentElement.val() || contentElement.text() || 'test';

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

        function getReadabilityScore(_content) {
            // Data to send
            var dataToSend = {
                content: _content
            };

            // Make the POST request
            $.post("/getreadabaility", dataToSend)
                .done(function (response) {
                    analysisResults.readability = response;
                    //$.each(response, function (key, value) {
                    //    $('#keyword_result').append(`<li>${key} (${value})</li><br>`);
                    //});
                })
                .fail(function (xhr) {
                    alert("Error: " + xhr.responseText);
                });
        }

        function countSyllables(word) {
            return word.toLowerCase().match(/[aeiouy]{1,2}/g)?.length || 1;
        }

        function getKeywordDensity(content, keyword) {
            if (!keyword) return 0;
            return (content.match(new RegExp(`\\b${keyword}\\b`, 'gi')) || []).length;
        }
        function getSeoAnylisisi() {
            
            var dataToSend = {
                focusKeyphrase: settings.seo.focusKeyphrase||'',
                seoTitle: settings.seo.seoTitle || '',
                slug: settings.seo.slug || '',
                metaDescription: settings.seo.metaDescription || '', 
                content: _htmlcontent || ''
            };

            // Make the POST request
            $.post("/getseoanylisis", dataToSend)
                .done(function (response) {
                    
                    analysisResults.seoAnylisis = response;
                    
                })
                .fail(function (xhr) {
                    alert("Error: " + xhr.responseText);
                });

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
        function initAllService() {
            getSeoAnylisisi()
            getWordCount(content, settings.stopWords);
            getReadabilityScore(content);
            getKeywordDensity(content, settings.focusKeyword);
        }
        var analysisResults = {
            seoAnylisis: {},
            topWords: {},
            readabilityScore: {},
            keywordDensity: {}
        };
        if (settings.editor) {
            _htmlcontent = settings.editor.getData();
            settings.editor.model.document.on('change:data', () => {
                clearTimeout(typingTimer);
                typingTimer = setTimeout(() => {
                    initAllService();

                }, settings.delayTimeout);
            });
        }
        initAllService();
        return {
            keywordDensity: function (callback) {
                if (typeof callback === 'function') {
                    callback(analysisResults.keywordDensity);
                }
                return this;
            },
            readability: function (callback) {
                if (typeof callback === 'function') {
                    callback(analysisResults.readability);
                }
                return this;
            },
            focusKeyword: function (callback) {
                if (typeof callback === 'function') {
                    callback(analysisResults.topWords);
                }
                return this;
            },
            getSeoAnylisis: function (callback) {
                if (typeof callback === 'function') {
                    callback(analysisResults.seoAnylisis);
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
