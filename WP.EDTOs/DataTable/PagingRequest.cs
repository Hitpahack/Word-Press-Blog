using Newtonsoft.Json;

namespace jQueryDatatable
{
    public class PagingRequest
    {
       
        public PagingRequest()
        {
            this.Columns = new List<Column>();
            this.Order = new List<Order>();
            this.SearchCriteria = new SearchCriteria();
        }
        [JsonProperty("draw")]
        public int Draw { get; set; }

        [JsonProperty("columns")]
        public IList<Column> Columns { get; set; }

        [JsonProperty("order")]
        public IList<Order> Order { get; set; }

        private int _start =0;
        [JsonProperty("start")]
        public int start
        {
            get
            {

                if (PageNo>0)
                    return PageNo;

                return _start;
            }
            set
            {
                _start = value;

            }
        }

        private int _length = 10;
        [JsonProperty("length")]
        public int length
        {
            get
            {

                if (RecordPerPage > 0)
                    return RecordPerPage;

                return _length;
            }
            set
            {
                _length = value;

            }
        }

        [JsonProperty("search")]
        public Search Search { get; set; }
        [JsonProperty("searchText")]
        public string SearchText => Search?.Value;

        [JsonProperty("searchCriteria")]
        public SearchCriteria SearchCriteria { get; set; }

        public int _pageNo { get { return start / length; } }
        public int _recordPerPage { get { return length; } }

        [JsonProperty("pageNo")]
        int PageNo { get; set; }
        [JsonProperty("recordPerPage")]
        int RecordPerPage { get; set; }
        [JsonProperty("syncServer")]
        public bool SyncServer { get; set; }

    
    }


  

}
