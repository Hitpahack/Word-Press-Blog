using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.EDTOs.Yoast
{
    public class YOAST_DTO
    {
        public string resultflat { get; set; } // good, improvements, problems
        public string result_message { get; set; } 
        public string title { get; set; } 
        public string url { get; set; } 
        public string description { get; set; } 
    }
}
