using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WP.Common;

namespace WP.EDTOs.Categories
{
    public class BASE_TERMS_DT_RESPONSE
    {

    }
    public class CATEGORIES_TERMS_DT_RESPONSE : BASE_TERMS_DT_RESPONSE
    {
        public string Most_Used_Category { get; set; }  

    }
    public class TAGS_TERMS_DT_RESPONSE : BASE_TERMS_DT_RESPONSE
    {
       
        public string Most_Used_Tag { get; set; }

    }
}
