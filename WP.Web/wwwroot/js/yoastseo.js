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
                
            },
            onSeoCallback: null,
            onReadabilityCallback: null
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
        let observable = {
            readability: null,
            seo_analysis: null,
            readability_score: null,
            seo_analysis_score: null,
            focus_keyphrase: null,
            keyword_density: null,
        }
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
                    $('#readability_score_box,#gotoreadabilitytab').removeAttr('class');
                    $('#readability_score_box').text('Not analyzed');
                    $('#Seo_ReadabilityScore').val('');

                    if (readaility_score) {
                        $('#readability_score_box,#gotoreadabilitytab').removeClass();
                        $('#readability_score_box,#gotoreadabilitytab').addClass(readaility_score);

                        $('#readability_score_box').text(readaility_score);
                        $('#Seo_ReadabilityScore').val(readaility_score.toLowerCase().replace(" ", "_"));
                    }
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
                    $('#gotoseotab,#seo_score_box').removeAttr('class');
                    $('#seo_score_box').text('Not analyzed');
                    $('#Seo_SeoScore').val('');
                    if (seo_score) {
                        $('#gotoseotab,#seo_score_box').addClass(seo_score);
                        $('#seo_score_box').text(seo_score);
                        $('#Seo_SeoScore').val(seo_score.toLowerCase().replace(" ", "_"));
                    }
                    else {

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
            observable.seo_analysis = getSeoAnalysisObservable();
            observable.focus_keyphrase = getFocusKeyphraseObservable(content);
            observable.readability = getReadabilityAnalysisObservable(content);
            observable.keyword_density = getKeywordDensityObservable(content, settings.seo.focusKeyword);
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

        getReadabilityAnalysisObservable = (_content) => {
            return new rxjs.Observable(observer => {
                // Data to send
                var dataToSend = { content: _content };

                // Make the POST request
                $.post("/getreadabaility", dataToSend)
                    .done(function (response) {
                        //analysisResults.readability = response;
                        var readability_score = getScore_readability(response);
                        observable.readability_score = readability_score;

                        //$('#readability_score_box,#gotoreadabilitytab').removeAttr('class');
                        //$('#readability_score_box').text('Not analyzed');
                        //$('#Seo_ReadabilityScore').val('');

                        //if (readability_score) {
                        //    $('#readability_score_box,#gotoreadabilitytab').removeClass();
                        //    $('#readability_score_box,#gotoreadabilitytab').addClass(readability_score);

                        //    $('#readability_score_box').text(readability_score);
                        //    $('#Seo_ReadabilityScore').val(readability_score.toLowerCase().replace(" ", "_"));
                        //}

                        // Emit the response
                        observer.next({
                            data: response,
                            score: readability_score
                        });
                        observer.complete();
                    })
                    .fail(function (xhr) {
                        observer.error("Error: " + xhr.responseText);
                    });
            });
        }
        getSeoAnalysisObservable = () => {
            return new rxjs.Observable(observer => {
                // Make the POST request
                $.post("/getseoanylisis", seoAnalysisReqData)
                    .done(function (response) {
                      
                        var seo_score = getScore_seoanylisis(response);
                        observable.seo_analysis_score = seo_score;

                        //$('#gotoseotab,#seo_score_box').removeAttr('class');
                        //$('#seo_score_box').text('Not analyzed');
                        //$('#Seo_SeoScore').val('');

                        //if (seo_score) {
                        //    $('#gotoseotab,#seo_score_box').addClass(seo_score);
                        //    $('#seo_score_box').text(seo_score);
                        //    $('#Seo_SeoScore').val(seo_score.toLowerCase().replace(" ", "_"));
                        //}

                        // Emit the response
                        observer.next({
                            data: response,
                            score: seo_score
                        });
                        observer.complete();
                    })
                    .fail(function (xhr) {
                        observer.error("Error: " + xhr.responseText);
                    });
            });
        }
        getFocusKeyphraseObservable = (_content) => {
            return new rxjs.Observable(observer => {
                var dataToSend = { content: _content };
                // Make the POST request
                $.post("/getfocuskeyphrase", dataToSend)
                    .done(function (response) {
                        var result = generateSEOKeyphrase(response);
                        analysisResults.topWords = result;

                        // Emit the result
                        observer.next(result);
                        observer.complete();
                    })
                    .fail(function (xhr) {
                        observer.error("Error: " + xhr.responseText);
                    });
            });
        }
        getKeywordDensityObservable = (content, keyword) => {
            return new rxjs.Observable(observer => {
                if (!keyword) {
                    observer.next(0);  // Emit 0 if no keyword is provided
                    observer.complete();
                    return;
                }

                try {
                    // Count occurrences of the keyword in the content
                    let count = (content.match(new RegExp(`\\b${keyword}\\b`, 'gi')) || []).length;

                    // Emit the count
                    observer.next(count);
                    observer.complete();
                } catch (error) {
                    observer.error("Error calculating keyword density: " + error);
                }
            });
        }

        var analysisResults = {
            seoAnylisis: {},
            topWords: {},
            readabilityScore: {},
            keywordDensity: {},
            seo_rate: 'Not analyzed',
            readability_rate: 'Not analyzed'
        };
        let obs_readability_score;
        if (settings.editor) {
            content = settings.editor.getData();
            observable.readability = getReadabilityAnalysisObservable(content);
            initAllService();
            settings.editor.model.document.on('change:data', () => {
                content = settings.editor.getData();
                clearTimeout(typingTimer);
                typingTimer = setTimeout(() => {
                    observable.readability = getReadabilityAnalysisObservable(content);
                    observable.readability.subscribe();
                    observable.focus_keyphrase = getFocusKeyphraseObservable(content);

                    if (settings.onSeoCallback)
                        settings.onSeoCallback();
                    if (settings.onReadabilityCallback)
                        settings.onReadabilityCallback();

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
                        observable.readability = getReadabilityAnalysisObservable(content);
                        observable.seo_analysis = getSeoAnalysisObservable();
                        observable.seo_analysis.subscribe();

                        if (settings.onSeoCallback)
                            settings.onSeoCallback();
                        if (settings.onReadabilityCallback)
                            settings.onReadabilityCallback();

                    }, settings.delayTimeout);
                });

        }
        setTimeout(() => {
            if (settings.onSeoCallback)
                settings.onSeoCallback();
            if (settings.onReadabilityCallback)
                settings.onReadabilityCallback();
        },1000)
        return {
            keywordDensity: function (callback) {
                if (typeof callback === 'function') {
                    observable.keyword_density.subscribe({
                        next: (score) => callback(score),
                        error: (err) => console.error(err),
                        complete: () => console.log("Seo analysis completed.")
                    });
                }
                return this;
            },
            readability: function (callback) {
                if (typeof callback === 'function') {
                    //callback(analysisResults.readability);
                    observable.readability.subscribe({
                        next: (response) => callback(response.data),
                        error: (err) => console.error(err),
                        complete: () => console.log("Readability analysis completed.")
                    });
                }
                return this;
            },
            focusKeyword: function (callback) {
                if (typeof callback === 'function') {
                    observable.focus_keyphrase.subscribe({
                        next: (score) => callback(score),
                        error: (err) => console.error(err),
                        complete: () => console.log("Focus keyword completed.")
                    });
                }
                return this;
            },
            getSeoAnylisis: function (callback) {
                if (typeof callback === 'function') {
                    observable.seo_analysis.subscribe({
                        next: (response) => callback(response.data),
                        error: (err) => console.error(err),
                        complete: () => console.log("Seo analysis completed.")
                    });
                }
                return this;
            },
            getSeo_Score: function (callback) {
                if (typeof callback === 'function') {
                    callback({
                        seo_rate: observable.seo_analysis_score,
                        readability_rate: observable.readability_score
                    });
                }
                return this;
            },
            generate_Seo_Score: function (callback) {
                observable.seo_analysis = getSeoAnalysisObservable();
                observable.readability = getReadabilityAnalysisObservable(content);
                rxjs.forkJoin([observable.seo_analysis, observable.readability]).subscribe(
                    ([seoResult, readabilityResult]) => {
                        callback({
                            seo_rate: seoResult.score,
                            readability_rate: readabilityResult.score
                        });
                    },
                    error => {
                        console.error("", error);
                    }
                );

                
                return this;
            }

        };
    }

    // Load YoastSEO globally
    global.yoastSEO = function (options) {
        return new YoastSEO(options);
    };
})(window, jQuery);
