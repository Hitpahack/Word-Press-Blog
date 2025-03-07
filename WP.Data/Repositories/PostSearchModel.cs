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
        public WpUser WpUser { get; set; }
        public WpTermRelationship PostRel { get; set; }
        public WpTermTaxonomy TermTaxonomy { get; set; }
        public WpTerm WpTerm { get; set; }
        public WpPostmetum Postmetum { get; set; }
        public WpPost FeatiredImage { get; set; }


    }

    public partial class PostResponseDto
    {
        public PostResponseDto()
        {

        }
        public ulong Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string Author { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        


    }
}
