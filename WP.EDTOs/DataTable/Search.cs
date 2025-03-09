using Newtonsoft.Json;
using System.Collections.Generic;

namespace jQueryDatatable
{
    public class Search
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("regex")]
        public bool Regex { get; set; }
    }

    public class Search2
    {
        [JsonProperty("value")]
        public string Value { get; set; }
        public SearchFilter SearchFilter
        {
            get
            {
                if (!string.IsNullOrEmpty(Value) && Value.StartsWith('{') && Value.EndsWith('}'))
                {
                    return JsonConvert.DeserializeObject<SearchFilter>(Value);
                }
                return null;
            }
        }

        [JsonProperty("regex")]
        public bool Regex { get; set; }
    }

    public class SearchFilter
    {
        public bool MatchAll { get; set; }
        public bool MatchAny { get; set; }
        public IList<ColumnFilter> filters { get; set; }
        public string[] SearchInProperty { get; set; }
      
    }

    public class ColumnFilter
    {
        public QueryType Query { get; set; }
        public object Value { get; set; }
    }

    public class QueryType
    {
        public string EnumType { get; set; }
        public int Value { get; set; }
    }

}
