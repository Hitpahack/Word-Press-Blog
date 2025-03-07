using jQueryDatatable;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.DTOs
{
    public class SearchModel : PagingRequest
    {
        [JsonProperty("status")]
        public string? Status { get; set; }
        [JsonProperty("date")]
        public string? Date { get; set; }
        [JsonProperty("categoryId")]
        public ulong? CategoryId { get; set; }
        [JsonProperty("rankMathFilter")]
        public string? RankMathFilter { get; set; }
        public int? Page => (start/ length)+1;
        public int? PageSize => length;
        public DateTime DateTime
        {
            get
            {
                if (!string.IsNullOrEmpty(Date))
                    return ParseYearMonth(Date);
                else
                    return DateTime.Now;
            }
        }

        internal  DateTime ParseYearMonth(string yearMonth)
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

            return new DateTime(year, month, 1); // Set day to 1

        }
    }
    
}


