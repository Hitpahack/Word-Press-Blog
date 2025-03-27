(function (global, $) {
    function YoastSEO(options) {
        var settings = $.extend({
            contentSelector: '',  // Required: Set the content input
            editor: null,  // Required: Set the content input
            stopWords: ["the", "and", "end", "these", "those", "is", "are", "was", "were", "this", "that", "to", "in", "on", "for", "with", "as", "of", "at", "by", "an", "a", "it", "has", "you", "am", "he", "she"],
            delayTimeout: 2000,
            seo: {
                focusKeyphrase: null,
                seoTitle: null,
                slug: null,
                metaDescription: null
            }
        }, options);

        let typingTimer;
        let typingTimer2;
        let content;
        let seoAnalysisReqData = {
            focusKeyphrase: settings.seo.focusKeyphrase ? () => $(settings.seo.focusKeyphrase).val() : '',
            seoTitle: settings.seo.seoTitle ? () => $(settings.seo.seoTitle).val() : '',
            slug: settings.seo.slug ? () => $(settings.seo.slug).val() : '',
            metaDescription: settings.seo.metaDescription ? () => $(settings.seo.metaDescription).val() : '',
            content: () => content || ''
        };
        if (!settings.editor)
            throw "editor required";

        var contentElement = $(settings.contentSelector);
        if (!contentElement.length) {
            console.warn("YoastSEO: contentSelector not found!");
            return;
        }

        function getfocuskeyphrase(_content, stopWords) {

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
                    var readaility_score = getScore_readability(response);
                    analysisResults.readability_rate = readaility_score;
                    if (readaility_score)
                        $('#readability_score_box,#gotoreadabilitytab').removeClass();
                    $('#readability_score_box,#gotoreadabilitytab').addClass(readaility_score);

                    $('#readability_score_box').text(readaility_score);
                    $('#Seo_ReadabilityScore').val(readaility_score.toLowerCase().replace(" ", "_"));
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

            // Make the POST request
            $.post("/getseoanylisis", seoAnalysisReqData)
                .done(function (response) {

                    analysisResults.seoAnylisis = response;

                    var seo_score = getScore_seoanylisis(response);
                    analysisResults.seo_rate = seo_score;
                    if (seo_score) {
                        $('#gotoseotab,#seo_score_box').removeClass();
                        $('#gotoseotab,#seo_score_box').addClass(seo_score);
                        $('#seo_score_box').text(seo_score);
                        $('#Seo_SeoScore').val(seo_score.toLowerCase().replace(" ", "_"));
                    }
                })
                .fail(function (xhr) {
                    alert("Error: " + xhr.responseText);
                });

        }
        function generateSEOKeyphrase(response) {
            // Convert response object into an array of key-value pairs
            let wordEntries = Object.entries(response);
            let keyphrase = '';

            // Sort words based on their score (higher first)
            wordEntries.sort((a, b) => b[1] - a[1]);

            if (wordEntries.length > 0) {
                // Determine a dynamic threshold for primary vs secondary
                let maxScore = wordEntries[0][1];  // Highest score in the list
                let threshold = maxScore * 0.5;  // Set threshold as 50% of the highest score

                let primaryKeywords = [];
                let secondaryKeywords = [];

                wordEntries.forEach(([word, score]) => {
                    if (score >= threshold) {
                        primaryKeywords.push(`<b class='primary_word'>${word}</b>`);
                    } else {
                        secondaryKeywords.push(`<b class='secondary_word'>${word}</b>`);
                    }
                });

                // Construct SEO-friendly keyphrase
                keyphrase = primaryKeywords.join(" ") + (secondaryKeywords.length > 0 ? " - " + secondaryKeywords.join(" ") : "");
            }


            return keyphrase || "Trending Topics & Styles"; // Fallback if no keywords found
        }
        function initAllService() {
            getSeoAnylisisi()
            getfocuskeyphrase(content, settings.stopWords);
            getReadabilityScore(content);
            getKeywordDensity(content, settings.focusKeyword);
        }
        function getScore_seoanylisis(seoData) {
            let scores = {
                good: Object.keys(seoData.good || {}).length,
                improvement: Object.keys(seoData.improvement || {}).length,
                problems: Object.keys(seoData.problems || {}).length
            };
            var result = "Not analyzed";
            // Determine the highest category, prioritizing "problems" over "improvement" if they are equal
            if (scores.problems > 2) {
                result = "problems";
            }
            else {
                if (scores.improvement > scores.good) {
                    result = "improvement";
                } else {
                    result = "good";
                }
            }
            // Find the category with the highest count
            //let highestCategory = Object.keys(scores).reduce((a, b) => scores[a] >= scores[b] ? a : b);

            return result;
        }
        function getScore_readability(data) {
            let scores = { good: 0, problems: 0, improvement: 0 };

            if (data.length > 0) {
                // Count occurrences of each resultflag
                data.forEach(item => {
                    if (scores.hasOwnProperty(item.resultflag)) {
                        scores[item.resultflag]++;
                    }
                });

                var result = "Not analyzed";
                // Determine the highest category, prioritizing "problems" over "improvement" if they are equal
                if (scores.problems > 2) {
                    result = "problems";
                }
                else {
                    if (scores.improvement > scores.good) {
                        result = "improvement";
                    }
                    else {
                        result = "good";
                    }
                }

            }
            return result;
        }


        var analysisResults = {
            seoAnylisis: {},
            topWords: {},
            readabilityScore: {},
            keywordDensity: {},
            seo_rate: 'Not analyzed',
            readability_rate: 'Not analyzed'
        };
        if (settings.editor) {
            content = settings.editor.getData();
            initAllService();
            settings.editor.model.document.on('change:data', () => {
                content = settings.editor.getData();
                clearTimeout(typingTimer);
                typingTimer = setTimeout(() => {
                    getReadabilityScore(content);
                    getfocuskeyphrase(content, settings.stopWords);
                }, settings.delayTimeout);
            });

            $([])
                .add(settings.seo.focusKeyphrase)
                .add(settings.seo.seoTitle)
                .add(settings.seo.slug)
                .add(settings.seo.metaDescription)
                .on('change', function () {
                    clearTimeout(typingTimer2);
                    typingTimer2 = setTimeout(() => {
                        getReadabilityScore(content);
                        getSeoAnylisisi();
                    }, settings.delayTimeout);
                });

        }

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
            },
            getSeo_Score: function (callback) {
                if (typeof callback === 'function') {
                    callback({
                        seo_rate: analysisResults.seo_rate,
                        readability_rate: analysisResults.readability_rate
                    });
                }
                return this;
            },
            generate_Seo_Score: function (callback) {
                getSeoAnylisisi();
                getSeo_Score(callback);
                return this;
            }

        };
    }

    // Load YoastSEO globally
    global.yoastSEO = function (options) {
        return new YoastSEO(options);
    };
})(window, jQuery);
