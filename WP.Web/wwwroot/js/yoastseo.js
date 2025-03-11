
(function ($) {
    $.fn.yoastseo = function (options) {
        var settings = $.extend({
            stopWords: ["the", "and", "end", "these", "those", "is", "are", "was", "were", "this", "that", "to", "in", "on", "for", "with", "as", "of", "at", "by", "an", "a", "it", "has", "you", "am", "he", "she"] // Default empty stop words list
        }, options);

        function getTopWords(content, stopWords) {
            const words = content.toLowerCase().split(/\W+/)
                .filter(w => w.length > 2 && !stopWords.includes(w));

            const wordCounts = words.reduce((acc, word) => {
                acc[word] = (acc[word] || 0) + 1;
                return acc;
            }, {});

            return Object.entries(wordCounts)
                .sort((a, b) => b[1] - a[1])
                .slice(0, 10)
                .reduce((acc, [key, value]) => {
                    acc[key] = value;
                    return acc;
                }, {});
        }

        return this.each(function () {
            var $this = $(this);
            var content = $this.val() || $this.text(); // Get text from input/textarea or element
            var topWords = getTopWords(content, settings.stopWords);

            // Ensure the event is triggered properly
            setTimeout(() => {
                $this.trigger("yoastseo:processed", [topWords]);
            }, 0);
        });
    };
}(jQuery));
