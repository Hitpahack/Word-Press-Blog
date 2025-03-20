using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WP.EDTOs.Medias
{
    public class WP_POST_MEDIA_ADD
    {
        [Required]
        public string Post_Title { get; set; }
        [Required]
        public string Post_Name { get; set; }
        public DateTime? Post_Date { get; set; }
        public DateTime? Post_Date_Gmt { get; set; }
        public ulong? Post_Author { get; set; }
    }
}
