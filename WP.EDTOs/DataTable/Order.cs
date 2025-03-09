using Newtonsoft.Json;

namespace jQueryDatatable
{
    public class Order
    {
        [JsonProperty("column")]
        public int Column { get; set; }

        [JsonProperty("dir")]
        public string Dir { get; set; }
    }
}
