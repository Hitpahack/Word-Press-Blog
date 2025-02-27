using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.DTOs
{
    public class MediaMetadata
    {
        public int width { get; set; }
        public int height { get; set; }
        public string file { get; set; }
        public MediaSizes sizes { get; set; }
    }

    public class MediaSizes
    {
        public MediaSize thumbnail { get; set; }
        public MediaSize medium { get; set; }
        public MediaSize large { get; set; }
    }

    public class MediaSize
    {
        public string file { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

}
