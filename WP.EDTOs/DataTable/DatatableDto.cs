namespace jQueryDatatable
{

    public class Datatable<T> where T : class
    {
        public Datatable(IEnumerable<T> data, int draw, int totalitem, int filteritem)
        {
            Draw = draw;
            RecordsTotal = totalitem;
            RecordsFiltered = filteritem;
            Data = data;
        }
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public IEnumerable<T> Data { get; set; }

    }
}
