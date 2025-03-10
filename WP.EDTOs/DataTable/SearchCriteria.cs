using Newtonsoft.Json;
using System;

namespace jQueryDatatable
{
    public class SearchCriteria
    {
        [JsonProperty("filter")]
        public object Filter { get; set; }

        [JsonProperty("isPageLoad")]
        public bool IsPageLoad { get; set; }
        [JsonProperty("From")]
        public DateTime? From { get; set; }
        [JsonProperty("To")]
        public DateTime? To { get; set; }
        [JsonProperty("tableTree")]
        public bool TableTree { get; set; }
    }
}
