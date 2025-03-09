using jQueryDatatable;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.EDTOs.Categories
{
   
    public class TermsPagingRequest : PagingRequest
    {
        [JsonProperty("status")]
        // true/false eg: 0 or 1
        public string? Status { get; set; }
        public string? Date { get; set; }
        public int? Page => (start / length) + 1;
        public int? PageSize => length;
        public string? FDate
        {
            get
            {
                if (!string.IsNullOrEmpty(Date) && Date.ToLower() != "all")
                    return ParseYearMonth(Date);
                else
                    return "";
            }
        }
        internal string ParseYearMonth(string yearMonth)
        {
            // Ensure the input is valid (length 6, numeric)
            if (string.IsNullOrWhiteSpace(yearMonth) || yearMonth.Length != 6 || !int.TryParse(yearMonth, out int ym))
                throw new FormatException($"Invalid YearMonth format: {yearMonth}");

            // Extract Year and Month
            int year = ym / 100;  // First 4 digits are the year
            int month = ym % 100; // Last 2 digits are the month

            // Validate month range (1-12)
            if (month < 1 || month > 12)
                throw new FormatException($"Invalid month in YearMonth: {yearMonth}");

            return new DateTime(year, month, 1).ToString("yyyy-mm-dd"); // Set day to 1

        }
    }
}
