using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.EDTOs
{
   
    public class POST_SP_RESPONSE : SP_BASE_RESPONSE
    {
        public int Id { get; set; }
        public string? Post_Title { get; set; }
        public string? Post_Name { get; set; }
        public string? Post_Content { get; set; }
        public string? Post_Status { get; set; }
        public DateTime? Post_Date { get; set; }
        public DateTime? Post_Date_Gmt { get; set; }
        public ulong? Post_Author { get; set; }
        public string? User_Login { get; set; }
        public string? featured_image_url { get; set; }

        // Categories and Tags are concatenated strings from GROUP_CONCAT in the query
        /// <summary>
        /// Comma seperated values
        /// </summary>
        public string? Categories { get; set; }
        /// <summary>
        /// Comma seperated value
        /// </summary>
        public string? Tags { get; set; }


    }
}
