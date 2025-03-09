using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.Common;

namespace WP.EDTOs
{
    public class POST_DT_RESPONSE : BASE_SP_RESPONSE
    {
        public ulong Id { get; set; }
        public string? Categories { get; set; }
        public string? Tags { get; set; }
               
    }
}
