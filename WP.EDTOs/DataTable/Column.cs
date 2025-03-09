using Newtonsoft.Json;

namespace jQueryDatatable
{
    public class Column
    {
        [JsonProperty( "data")]
        public string Data { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("searchable")]
        public bool Searchable { get; set; }

        [JsonProperty("orderable")]
        public bool Orderable { get; set; }

        [JsonProperty("search")]
        public Search2 Search { get; set; }
    }
}
