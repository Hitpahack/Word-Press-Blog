using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.Data.Repositories
{
    public partial class PostSearchDto
    {
        public PostSearchDto()
        {
                
        }
        public WpPost Post { get; set; }
        public WpTermRelationship PostRel { get; set; }
        public WpPostmetum Postmetum { get; set; }
        public WpTerm WpTerm { get; set; }
        public WpUser WpUser { get; set; }
    }
}
